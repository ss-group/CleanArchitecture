package com.ss.spa.model.gs;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)
public class GSRP05ReportModel extends BaseModel {
    // ----------required
    private String reportName;
    private String exportType;
	// ---------- use in Ireport 
    private String userName;
    private String langCode;
    private int graduateAcademicYear;
    private int graduateSemester;
	private String programCode;
	private String facCode;
    private String companyCode;
}
