package com.ss.spa.model.gs;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)
public class GSRP01ReportModel extends BaseModel {
    // ----------required
    private String reportName;
    private String exportType;
    // ---------- use in Ireport 
	private String lang;
    private String user;
    private String companyCode;
	private int academicYear;
	private int academicSemester;
	private String programCode;
	private String studentTypeCode;
	private String reportType;
}
