package com.ss.spa.core.report;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.sql.Connection;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import javax.sql.DataSource;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Component;

import lombok.extern.slf4j.Slf4j;
import net.sf.jasperreports.engine.JRException;
import net.sf.jasperreports.engine.JasperCompileManager;
import net.sf.jasperreports.engine.JasperFillManager;
import net.sf.jasperreports.engine.JasperPrint;
import net.sf.jasperreports.engine.JasperReport;
import net.sf.jasperreports.engine.export.HtmlExporter;
import net.sf.jasperreports.engine.export.JRCsvExporter;
import net.sf.jasperreports.engine.export.JRPdfExporter;
import net.sf.jasperreports.engine.export.JRXmlExporter;
import net.sf.jasperreports.engine.export.ooxml.JRXlsxExporter;
import net.sf.jasperreports.engine.util.JRLoader;
import net.sf.jasperreports.engine.util.JRSaver;
import net.sf.jasperreports.export.Exporter;
import net.sf.jasperreports.export.SimpleExporterInput;
import net.sf.jasperreports.export.SimpleHtmlExporterOutput;
import net.sf.jasperreports.export.SimpleOutputStreamExporterOutput;
import net.sf.jasperreports.export.SimpleWriterExporterOutput;
import net.sf.jasperreports.export.SimpleXmlExporterOutput;

@Slf4j
@Component
public class JasperReportsService implements ReportService {

	@Autowired
	private FileSystemStorageService storageService;

	@Autowired
	private JasperReportsService reportService;

	@Autowired
	private DataSource dataSource;

	@Override
	public ResponseEntity<byte[]> generateReport(String inputFileName, String exportType, Map<String, Object> params)
			throws JRException, SQLException {

		HttpHeaders httpHeaders = new HttpHeaders();
		ContentDisposition contentDisposition = ContentDisposition.builder("inline")
				.filename(inputFileName + reportService.getFileExtension(exportType)).build();
		httpHeaders.setContentType(MediaType.parseMediaType(reportService.getMediaType(exportType)));
		httpHeaders.setContentDisposition(contentDisposition);
		httpHeaders.setAccessControlAllowCredentials(true);
		List<String> exposedHeaders = new ArrayList<String>();
		exposedHeaders.add("Content-Disposition");
		httpHeaders.setAccessControlExposeHeaders(exposedHeaders);

		return ResponseEntity.ok().headers(httpHeaders).body(generate(inputFileName, exportType, params));
	}

	@Override
	public byte[] generate(String inputFileName, String exportType, Map<String, Object> params) throws SQLException {
		log.info("generate :: FileName : {}", inputFileName);

		byte[] bytes = null;
		Connection connection = null;
		JasperReport jasperReport = null;
		try (ByteArrayOutputStream byteArray = new ByteArrayOutputStream()) {
			// Check if a compiled report exists
			if (storageService.jasperFileExists(inputFileName)) {
				jasperReport = (JasperReport) JRLoader.loadObject(storageService.loadJasperFile(inputFileName));
			}
			// Compile report from source and save
			else {
				String jrxml = storageService.loadJrxmlFile(inputFileName);
				jasperReport = JasperCompileManager.compileReport(jrxml);
				// Save compiled report. Compiled report is loaded next time
				JRSaver.saveObject(jasperReport, storageService.loadJasperFile(inputFileName));
			}
			connection = dataSource.getConnection();
			JasperPrint jasperPrint = JasperFillManager.fillReport(jasperReport, params, connection);
			bytes = export(jasperPrint, exportType);
		} catch (JRException | IOException e) {
			e.printStackTrace();
		} finally {
			if (connection != null) {
				connection.close();
			}
		}
		return bytes;
	}

	@Override
	@SuppressWarnings({ "unchecked", "rawtypes" })
	public byte[] export(final JasperPrint print, String exportType) throws JRException {
		final Exporter exporter;
		final ByteArrayOutputStream out = new ByteArrayOutputStream();
		switch (exportType) {
		case "HTML":
			exporter = new HtmlExporter();
			exporter.setExporterOutput(new SimpleHtmlExporterOutput(out));
			break;

		case "CSV":
			exporter = new JRCsvExporter();
			exporter.setExporterOutput(new SimpleWriterExporterOutput(out));
			break;

		case "XML":
			exporter = new JRXmlExporter();
			exporter.setExporterOutput(new SimpleXmlExporterOutput(out));
			break;

		case "XLSX":
			exporter = new JRXlsxExporter();
			exporter.setExporterOutput(new SimpleOutputStreamExporterOutput(out));
			break;

		case "PDF":
			exporter = new JRPdfExporter();
			exporter.setExporterOutput(new SimpleOutputStreamExporterOutput(out));
			break;

		default:
			throw new JRException("Unknown report format: " + exportType);
		}

		exporter.setExporterInput(new SimpleExporterInput(print));
		exporter.exportReport();

		return out.toByteArray();
	}

	@Override
	public String getFileExtension(String exportType) throws JRException {
		String result;
		switch (exportType) {
		case "HTML":
			result = ".html";
			break;

		case "CSV":
			result = ".csv";
			break;

		case "XML":
			result = ".xml";
			break;

		case "XLSX":
			result = ".xlsx";
			break;

		case "PDF":
			result = ".pdf";
			break;

		default:
			throw new JRException("Unknown report format: " + exportType);
		}

		return result;
	}

	@Override
	public String getMediaType(String exportType) throws JRException {
		String result;
		switch (exportType) {
		case "HTML":
			result = "text/html";
			break;

		case "CSV":
			result = "application/csv";
			break;

		case "XML":
			result = "application/xml";
			break;

		case "XLSX":
			result = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			break;

		case "PDF":
			result = "application/x-download";
			break;

		default:
			throw new JRException("Unknown report format: " + exportType);
		}

		return result;
	}
}
