package com.ss.spa.model;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)
public class Csrp01Model extends BaseModel {
	private String eduTypeLv;
	private String facCode;
	private String subjectCodeFrom;
	private String subjectCodeTo;
	private String academicYearFrom;
	private String academicYearTo;
	private String reportName;
	private String exportType;
	private String companyCode;
	private String langCode;
	private String publisher;
}
