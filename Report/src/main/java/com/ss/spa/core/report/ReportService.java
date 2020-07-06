package com.ss.spa.core.report;

import java.sql.SQLException;
import java.util.Map;
import org.springframework.http.ResponseEntity;
import net.sf.jasperreports.engine.JRException;
import net.sf.jasperreports.engine.JasperPrint;

public interface ReportService {

	public ResponseEntity<byte[]> generateReport(String inputFileName, String exportType, Map<String, Object> params) throws JRException, SQLException;
	
	public byte[] generate(String inputFileName, String exportType, Map<String, Object> params) throws SQLException;

	public byte[] export(JasperPrint print, String exportType) throws JRException;

	public String getFileExtension(String exportType) throws JRException;

	public String getMediaType(String exportType) throws JRException;

}
