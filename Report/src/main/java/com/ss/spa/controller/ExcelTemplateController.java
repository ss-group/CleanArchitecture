package com.ss.spa.controller;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.ss.spa.core.report.ExcelReportsService;
import com.ss.spa.model.Employee;
import com.ss.spa.model.Rp01ExcelModel;

import lombok.extern.slf4j.Slf4j;

@Slf4j
@RequestMapping(path = "/excel")
@CrossOrigin
@RestController
public class ExcelTemplateController {
	
	@Autowired
	ExcelReportsService excelReportsService;
	
	@PostMapping(path = "/rp01")
	public ResponseEntity<Object> rp01(@RequestBody Rp01ExcelModel model) throws Exception {
		log.info("excel Controller :: Param : {}", model);
//		String reportName = "object_collection_template";
//		String startPoint = "Template!A1";
		List<Employee> employees = generateSampleEmployeeData();
		Map<String, Object> params = new HashMap<String, Object>();
		params.put("employees", employees);
		
		return excelReportsService.generateReport(model.getReportName(), model.getStartPoint(), params);
	}

    private static List<Employee> generateSampleEmployeeData() throws ParseException {
        List<Employee> employees = new ArrayList<Employee>();
        SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MMM-dd", Locale.US);
        employees.add( new Employee("Elsa", dateFormat.parse("1970-Jul-10"), 1500, 0.15, 10) );
        employees.add( new Employee("Oleg", dateFormat.parse("1973-Apr-30"), 2300, 0.25, 10) );
        employees.add( new Employee("Neil", dateFormat.parse("1975-Oct-05"), 2500, 0.00, 10) );
        employees.add( new Employee("Maria", dateFormat.parse("1978-Jan-07"), 1700, 0.15, 10) );
        employees.add( new Employee("John", dateFormat.parse("1969-May-30"), 2800, 0.20, 10) );
        return employees;
    }

}
