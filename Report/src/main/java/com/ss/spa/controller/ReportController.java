package com.ss.spa.controller;

import java.io.FileNotFoundException;
import java.net.URL;
import java.sql.SQLException;
import java.util.HashMap;
import java.util.Map;

import com.ss.spa.model.fn.Fnod01ReportModel;
import com.ss.spa.model.fn.Fnod02ReportModel;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ModelAttribute;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.ss.spa.core.report.JasperReportsService;
//import com.ss.spa.model.cs.*;
import com.ss.spa.model.en.ENRP01ExcleModel;
import com.ss.spa.model.en.ENRP01PdfModel;
import com.ss.spa.model.en.ENRP02ExcleModel;
import com.ss.spa.model.en.ENRP03ExcleModel;
import com.ss.spa.model.en.ENRP04ExcleModel;
import com.ss.spa.model.en.ENRP05ExcleModel;
import com.ss.spa.model.en.ENRP06ExcleModel;
import com.ss.spa.model.fn.FNRP01Model;

import com.ss.spa.model.gs.GSRP01ReportModel;
import com.ss.spa.model.Gsrp02Model;
import com.ss.spa.model.ReportTestModel;
import com.ss.spa.model.Rp01Model;
import com.ss.spa.model.SHRP01Model;
import com.ss.spa.model.SHRP02Model;
import com.ss.spa.model.Shrp03Model;
import com.ss.spa.model.Shrp04Model;
import com.ss.spa.model.Shrp05Model;
import com.ss.spa.model.TrainJasperModel;

import com.ss.spa.model.gd.*;
import com.ss.spa.model.rg.*;
import com.ss.spa.model.gs.*;

import lombok.extern.slf4j.Slf4j;
import net.sf.jasperreports.engine.JRException;

@Slf4j
@RequestMapping(path = "/report")
@CrossOrigin
@RestController
public class ReportController {

	@Autowired
	private JasperReportsService reportService;

	@GetMapping(path = "/rp01")
	public ResponseEntity<byte[]> rp01Pdf(@ModelAttribute Rp01Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("rp01 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("table_name", model.getTableName());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

	@GetMapping(path = "/reportTest")
	public ResponseEntity<byte[]> reportTest(@ModelAttribute ReportTestModel model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportTest Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("lang_code", model.getLangCode());
		params.put("fac_code", model.getFacCode());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

	@PostMapping(path = "/reportDemo")
	public ResponseEntity<byte[]> reportDemo(@RequestBody ReportTestModel model) throws FileNotFoundException, JRException, SQLException {
		URL resource = getClass().getResource("/");
		String path = resource.getPath();
		log.info("reportDemo Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("sub_report_path", path);
		params.put("lang_code", model.getLangCode());
		params.put("fac_code", model.getFacCode());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

	@PostMapping(path = "/reportSHRP01")
	public ResponseEntity<byte[]> reportSHRP01(@RequestBody SHRP01Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportSHRP01 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("lang_code", model.getLangCode());
		params.put("fac_code", model.getFacCode());
		params.put("fac_code_to", model.getFacCodeTo());
		params.put("student_id", model.getStudentId());
		params.put("student_id_to", model.getStudentIdTo());
		params.put("program_code", model.getProgramCode());
		params.put("program_code_to", model.getProgramCodeTo());
		params.put("year", model.getRegisterYear());
		params.put("edu_type", model.getEducationTypeCode());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

	@PostMapping(path = "/reportSHRP02")
	public ResponseEntity<byte[]> reportSHRP02(@RequestBody SHRP02Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportSHRP02 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("lang_code", model.getLangCode());
		params.put("fac_code", model.getFacCode());
		params.put("fac_code_to", model.getFacCodeTo());
		params.put("program_code", model.getProgramCode());
		params.put("program_code_to", model.getProgramCodeTo());
		params.put("year", model.getRegisterYear());
		params.put("edu_type", model.getEducationTypeCode());
		params.put("student_status", model.getStudentStatus());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

	@GetMapping(path = "/trainJasper")
	public ResponseEntity<byte[]> trainJasper(@ModelAttribute TrainJasperModel model) throws FileNotFoundException, JRException, SQLException {
		log.info("trainJasper Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("fac_code", model.getFacCode());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}


	// @GetMapping(path = "/rp01testemp")
	// public ResponseEntity<byte[]> rp01Pdf(@ModelAttribute Rp01TestEmpModel model) throws FileNotFoundException, JRException, SQLException {
	// 	log.info("rp01testemp Controller :: Param : {}", model);

	// 	Map<String, Object> params = new HashMap<>();
	// 	params.put("companyCode", model.getCompanyCode());

	// 	return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	// }


	@PostMapping(path = "/reportENRP01")
	public ResponseEntity<byte[]> reportENRP01(@RequestBody ENRP01ExcleModel model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportENRP01 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("YEAR", model.getYear());
		params.put("SEMESTER", model.getSemester());
		params.put("EDUCATION_TYPE_LEVEL", model.getEducationTypeLevel());
		params.put("FAC_CODE", model.getFacCode());
		params.put("PROGRAM_CODE", model.getProgramCode());
		params.put("CURRICULUM", model.getCclId());
		params.put("EN_DATE_FROM", model.getStartDate());
		params.put("EN_DATE_TO", model.getToDate());
		params.put("EN_CODE_FROM", model.getExamineerIdFrom());
		params.put("EN_CODE_TO", model.getExamineerIdTo());
		params.put("MAJOR_CODE", model.getMajorCode());
		params.put("LANG", model.getLang());
		params.put("USER_NAME" , model.getUserName());
		params.put("COMPANY_CODE" , model.getCompanyCode());
		
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}
	@PostMapping(path = "/reportENRP01Pdf")
	public ResponseEntity<byte[]> reportENRP01Pdf(@RequestBody ENRP01PdfModel model) throws FileNotFoundException, JRException, SQLException {
		URL resource = getClass().getResource("/");
		String path = resource.getPath();
		log.info("reportENRP01Pdf Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("SUBREPORT_DIR", path);
		params.put("LANG", model.getLang());
		params.put("YEAR", model.getYear());
		params.put("SEMESTER", model.getSemester());
		params.put("EDUCATION_TYPE_LEVEL", model.getEducationTypeLevel());
		params.put("FAC_CODE", model.getFacCode());
		params.put("PROGRAM_CODE", model.getProgramCode());
		params.put("MAJOR_CODE", model.getMajorCode());
		params.put("CCL_ID", model.getCclId());
		params.put("EN_DATE_FROM", model.getStartDate());
		params.put("EN_DATE_TO", model.getToDate());
		params.put("EN_CODE_FROM", model.getExamineerIdFrom());
		params.put("EN_CODE_TO", model.getExamineerIdTo());
		params.put("COMPANY_CODE" , model.getCompanyCode());
		
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}
	@PostMapping(path = "/reportENRP02")
	public ResponseEntity<byte[]> reportENRP02(@RequestBody ENRP02ExcleModel model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportENRP02 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("YEAR", model.getYear());
		params.put("SEMESTER", model.getSemester());
		params.put("EDUCATION_TYPE_LEVEL", model.getEducationTypeLevel());
		params.put("CURRICULUM", model.getCclId());
		params.put("EN_CODE_FROM", model.getExamineerIdFrom());
		params.put("EN_CODE_TO", model.getExamineerIdTo());
		params.put("TAX_ID", model.getTaxId());
		params.put("LANG", model.getLang());
		params.put("USER_NAME" , model.getUserName());
		
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}
	
	@PostMapping(path = "/reportENRP03")
	public ResponseEntity<byte[]> reportENRP03(@RequestBody ENRP03ExcleModel model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportENRP03 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("YEAR", model.getYear());
		params.put("SEMESTER", model.getSemester());
		params.put("EDUCATION_TYPE_LEVEL", model.getEducationTypeLevel());
		params.put("CURRICULUM", model.getCclId());
		params.put("EN_CODE_FROM", model.getExamineerIdFrom());
		params.put("EN_CODE_TO", model.getExamineerIdTo());
		params.put("TAX_ID", model.getTaxId());
		params.put("LANG", model.getLang());
		params.put("USER_NAME" , model.getUserName());
		
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}
	
	@PostMapping(path = "/reportENRP04")
	public ResponseEntity<byte[]> reportENRP04(@RequestBody ENRP04ExcleModel model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportENRP04 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("YEAR", model.getYear());
		params.put("SEMESTER", model.getSemester());
		params.put("EDUCATION_TYPE_LEVEL", model.getEducationTypeLevel());
		params.put("CURRICULUM", model.getCclId());
		params.put("EN_CODE_FROM", model.getExamineerIdFrom());
		params.put("EN_CODE_TO", model.getExamineerIdTo());
		params.put("TAX_ID", model.getTaxId());
		params.put("LANG", model.getLang());
		params.put("USER_NAME" , model.getUserName());
		
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}
	
	@PostMapping(path = "/reportENRP05")
	public ResponseEntity<byte[]> reportENRP05(@RequestBody ENRP05ExcleModel model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportENRP05 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("YEAR", model.getYear());
		params.put("SEMESTER", model.getSemester());
		params.put("EDUCATION_TYPE_LEVEL", model.getEducationTypeLevel());
		params.put("CURRICULUM", model.getCclId());
		params.put("EN_CODE_FROM", model.getExamineerIdFrom());
		params.put("EN_CODE_TO", model.getExamineerIdTo());
		params.put("TAX_ID", model.getTaxId());
		params.put("LANG", model.getLang());
		params.put("USER_NAME" , model.getUserName());
		
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}
	
	@PostMapping(path = "/reportENRP06")
	public ResponseEntity<byte[]> reportENRP06(@RequestBody ENRP06ExcleModel model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportENRP06 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("YEAR", model.getYear());
		params.put("SEMESTER", model.getSemester());
		params.put("EDUCATION_TYPE_LEVEL", model.getEducationTypeLevel());
		params.put("CCL_ID", model.getCclId());
		params.put("EN_DATE_FROM", model.getStartDate());
		params.put("EN_DATE_TO", model.getToDate());
		params.put("LANG", model.getLang());
		params.put("USER_NAME" , model.getUserName());
		params.put("COMPANY_CODE" , model.getCompanyCode());
		
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}
	
	@PostMapping(path = "/rgrp01")
	public ResponseEntity<byte[]> rgrp01(@RequestBody Rgrp01Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("rgrp01 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("YEAR", model.getYear());
		params.put("SEMESTER", model.getSemester());
		params.put("EDUCATION_TYPE_LEVEL", model.getEducationTypeLevel());
		params.put("CCL_ID", model.getCclId());
		params.put("SUBJECT_DET_ID", model.getSubjectDetId());
		params.put("LANG_CODE", model.getLangCode());
		params.put("COMPANY_CODE", model.getCompanyCode());
		params.put("PUBLISHER" , model.getPublisher());
		
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

	@PostMapping(path = "/rgrp02")
	public ResponseEntity<byte[]> rgrp02(@RequestBody Rgrp02Model model) throws FileNotFoundException, JRException, SQLException {
		URL resource = getClass().getResource("/");
		String path = resource.getPath();
		log.info("rgrp02 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("YEAR", model.getYear());
		params.put("SEMESTER", model.getSemester());
		params.put("EDUCATION_TYPE_LEVEL", model.getEducationTypeLevel());
		params.put("FAC_CODE", model.getFacCode());
		params.put("LANG_CODE", model.getLangCode());
		params.put("COMPANY_CODE", model.getCompanyCode());
		params.put("PUBLISHER" , model.getPublisher());
		params.put("SUBREPORT_DIR", path);
		
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

	@PostMapping(path = "/rgrp03")
	public ResponseEntity<byte[]> rgrp03(@RequestBody Rgrp03Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("rgrp03 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("YEAR", model.getYear());
		params.put("SEMESTER", model.getSemester());
		params.put("EDUCATION_TYPE_LEVEL", model.getEducationTypeLevel());
		params.put("FAC_CODE", model.getFacCode());
		params.put("PROGRAM_CODE", model.getProgramCode());
		params.put("LANG_CODE", model.getLangCode());
		params.put("COMPANY_CODE", model.getCompanyCode());
		params.put("PUBLISHER" , model.getPublisher());
		
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

	// @PostMapping(path = "/csrp01")
	// public ResponseEntity<byte[]> csrp01(@RequestBody Csrp01Model model) throws FileNotFoundException, JRException, SQLException {
	// 	URL resource = getClass().getResource("/");
	// 	String path = resource.getPath();
	// 	log.info("csrp01 Controller :: Param : {}", model);

	// 	Map<String, Object> params = new HashMap<>();
	// 	params.put("EDUCATION_TYPE_LEVEL", model.getEduTypeLv());
	// 	params.put("FAC_CODE", model.getFacCode());
	// 	params.put("SUBJECT_CODE_FROM", model.getSubjectCodeFrom());
	// 	params.put("SUBJECT_CODE_TO", model.getSubjectCodeTo());
	// 	params.put("ACADEMIC_YEAR_FROM", model.getAcademicYearFrom());
	// 	params.put("ACADEMIC_YEAR_TO", model.getAcademicYearTo());
	// 	params.put("COMPANY_CODE", model.getCompanyCode());		
	// 	params.put("LANG_CODE", model.getLangCode());
	// 	params.put("SUBREPORT_DIR", path);
	// 	params.put("PUBLISHER", model.getPublisher());

	// 	return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	// }

    @PostMapping(path = "/reportGDRP01")
	public ResponseEntity<byte[]> reportGDRP01(@RequestBody Reportgdrp01Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportGDRP01 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("p_lin_id", model.getLangCode());
        params.put("p_year", model.getExamYear());
        params.put("p_semester", model.getExamSemesterSeq());
        params.put("p_education_type_level", model.getEducationTypeLevel());
        params.put("p_sub_code", model.getSubjCode());
        params.put("p_user_name", model.getUserName());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);

	}
	
	@PostMapping(path = "/shrp03")
	public ResponseEntity<byte[]> shrp03(@RequestBody Shrp03Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("shrp03 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		//params.put("lang_code", model.getLangCode());
		params.put("fac_code", model.getFacCode());
		params.put("fac_code_to", model.getFacCodeTo());
		params.put("program_code", model.getProgramCode());
		params.put("program_code_to", model.getProgramCodeTo());
		params.put("education_type_level", model.getEducationTypeLevel());
		params.put("register_year", model.getRegisterYear());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

	@PostMapping(path = "/shrp04")
	public ResponseEntity<byte[]> shrp04(@RequestBody Shrp04Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("shrp04 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		//params.put("lang_code", model.getLangCode());
		params.put("fac_code", model.getFacCode());
		params.put("fac_code_to", model.getFacCodeTo());
		params.put("program_code", model.getProgramCode());
		params.put("program_code_to", model.getProgramCodeTo());
		params.put("status_code", model.getStatusCode());
		params.put("status_code_to", model.getStatusCodeTo());
		params.put("education_type_level", model.getEducationTypeLevel());
		params.put("register_year", model.getRegisterYear());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

	@PostMapping(path = "/shrp05")
	public ResponseEntity<byte[]> shrp05(@RequestBody Shrp05Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("shrp05 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		//params.put("lang_code", model.getLangCode());
		params.put("education_type_level", model.getEducationTypeLevel());
		params.put("register_year", model.getRegisterYear());
		params.put("petition_code", model.getPetitionCode());
		params.put("petition_code_to", model.getPetitionCodeTo());
		params.put("pay_date", model.getPayDate());
		params.put("pay_date_to", model.getPayDateTo());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}
	
	@PostMapping(path = "/gsrp02")
	public ResponseEntity<byte[]> gsrp02(@RequestBody Gsrp02Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("shrp03 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("lang", model.getLang().toLowerCase());
		params.put("year", model.getYear());
		params.put("period", model.getPeriod());
		params.put("edu_type_level", model.getEduTypeLevel());
		params.put("from_program", model.getFromProgram());
		params.put("to_program", model.getToProgram());
		params.put("to_fac", model.getToFac());
		params.put("from_fac", model.getFromFac());
		params.put("from_student_code", model.getFromStudentCode());
		params.put("to_student_code", model.getToStudentCode());
		params.put("p_report_type", model.getType());
		params.put("company", model.getCompany());

		return reportService.generateReport("GSRP02", model.getExportType().toUpperCase(), params);
	}

	@PostMapping(path = "/reportFnod01")
	public ResponseEntity<byte[]> reportFnod01(@RequestBody Fnod01ReportModel model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportFnod01 Controller :: Param : {}", model);
		URL resource = getClass().getResource("/");
		String path = resource.getPath();
		Map<String, Object> params = new HashMap<>();
		params.put("lang", model.getLang());
		params.put("company_code", model.getCompanyCode());
		params.put("bill_m_id", model.getBillMId());
		params.put("report_path", path);
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

    @PostMapping(path = "/reportGdrp02")
	public ResponseEntity<byte[]> reportGdrp02(@RequestBody Reportgdrp02Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportGdrp02 Controller :: Param : {}", model);


		Map<String, Object> params = new HashMap<>();
		params.put("p_company_code", model.getCompany());
        params.put("p_education_type_level", model.getEducationTypeLevel());
        params.put("p_register_year", model.getRegisterYear());
        params.put("p_register_semester", model.getRegisterSemesterSeq());
        params.put("p_student_id", model.getStuId());
        params.put("p_lang", model.getLangCode());
        params.put("p_user", model.getUserName());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
    }
    
    @PostMapping(path = "/reportGdrp03")
	public ResponseEntity<byte[]> reportGdrp03(@RequestBody Reportgdrp03Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportGdrp03 Controller :: Param : {}", model);


		Map<String, Object> params = new HashMap<>();
		params.put("p_company_code", model.getCompany());
        params.put("p_education_type_level", model.getEducationTypeLevel());
        params.put("p_register_year", model.getRegisterYear());
        params.put("p_register_semester", model.getRegisterSemesterSeq());
        params.put("p_fac_code", model.getFacCode());
        params.put("p_lang", model.getLangCode());
        params.put("p_user", model.getUserName());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
    }
    
    @PostMapping(path = "/reportGdrp04")
	public ResponseEntity<byte[]> reportGdrp04(@RequestBody Reportgdrp04Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportGdrp04 Controller :: Param : {}", model);


		Map<String, Object> params = new HashMap<>();
		params.put("p_company_code", model.getCompany());
        params.put("p_education_type_level", model.getEducationTypeLevel());
        params.put("p_register_year", model.getRegisterYear());
        params.put("p_register_semester", model.getRegisterSemesterSeq());
        params.put("p_fac_code", model.getFacCode());
        params.put("p_class_no", model.getClassNo() == 0 ? null : model.getClassNo());
        params.put("p_lang", model.getLangCode());
        params.put("p_user", model.getUserName());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
    }
    
    @PostMapping(path = "/reportGdrp05")
	public ResponseEntity<byte[]> reportGdrp05(@RequestBody Reportgdrp05Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportGdrp05 Controller :: Param : {}", model);


		Map<String, Object> params = new HashMap<>();
		params.put("p_company_code", model.getCompany());
        params.put("p_education_type_level", model.getEducationTypeLevel());
        params.put("p_register_year", model.getRegisterYear());
        params.put("p_register_semester", model.getRegisterSemester());
        params.put("p_lang", model.getLangCode());
        params.put("p_user", model.getUserName());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

    @PostMapping(path = "/reportFNRP01")
	public ResponseEntity<byte[]> reportFNRP01(@RequestBody FNRP01Model model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportFNRP01 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("lang", model.getLang());
		params.put("company_code", model.getCompanyCode());
        params.put("system_code", model.getSystemCode());
        params.put("academic_year", model.getYear());
        params.put("academic_semester", model.getSemester());
        params.put("education_type_level", model.getEducationLv());
        params.put("education_type_level_to", model.getEducationLvTo());
		params.put("fac_code", model.getFacCode());
		params.put("fac_code_to", model.getFacCodeTo());
		params.put("program_code", model.getProCode());
		params.put("program_code_to", model.getProCodeTo());
        params.put("pricelist_code", model.getPricelistCode());
		params.put("pricelist_code_to", model.getPricelistCodeTo());
        params.put("reg_stype_code", model.getRegTypeCode());
		params.put("reg_stype_code_to", model.getRegTypeCodeTo());
        params.put("petition_code", model.getPetitionCode());
		params.put("petition_code_to", model.getPetitionCodeTo());
        params.put("USER_NAME" , model.getUserName());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}
	@PostMapping(path = "/reportFnod02")
	public ResponseEntity<byte[]> reportFnod02(@RequestBody Fnod02ReportModel model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportFnod02 Controller :: Param : {}", model);
		URL resource = getClass().getResource("/");
		String path = resource.getPath();
		Map<String, Object> params = new HashMap<>();
		params.put("lang", model.getLang());
		params.put("company_code", model.getCompanyCode());
		params.put("receive_no", model.getReceiveNo());
		params.put("doc_type", model.getDocType());
		params.put("sub_report_path", path);
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

	@PostMapping(path = "/reportGSRP05")
	public ResponseEntity<byte[]> reportGSRP05(@RequestBody GSRP05ReportModel model) throws FileNotFoundException, JRException, SQLException {
		log.info("reportGSRP05 Controller :: Param : {}", model);

		Map<String, Object> params = new HashMap<>();
		params.put("p_user", model.getUserName());
		params.put("p_lin_id", model.getLangCode());
        params.put("p_year", model.getGraduateAcademicYear());
        params.put("p_semester", model.getGraduateSemester());
        params.put("p_company_code", model.getCompanyCode());
        params.put("p_fac_code", model.getFacCode());
        params.put("p_program_code", model.getProgramCode());

		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}

	@PostMapping(path = "/reportGSRP01")
	public ResponseEntity<byte[]> reportGSRP01(@RequestBody GSRP01ReportModel model) throws FileNotFoundException, JRException, SQLException {
        log.info("reportGSRP01 Controller :: Param : {}", model);

        Map<String, Object> params = new HashMap<>();
		params.put("p_lang", model.getLang());
		//params.put("p_user", model.getUser());
        params.put("p_company_code", model.getCompanyCode());
        params.put("p_year", model.getAcademicYear());
        params.put("p_semester", model.getAcademicSemester());
        params.put("p_program_code", model.getProgramCode());
        params.put("p_student_type_code", model.getStudentTypeCode());
        params.put("report_type", model.getReportType());
		
		return reportService.generateReport(model.getReportName(), model.getExportType().toUpperCase(), params);
	}
}