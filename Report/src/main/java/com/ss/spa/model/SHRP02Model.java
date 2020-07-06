package com.ss.spa.model;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)
public class SHRP02Model extends BaseModel {
	private String facCode;
	private String facCodeTo;
	private String programCode;
    private String programCodeTo;
    private String educationTypeCode;
    private int registerYear;
	private String reportName;
	private String exportType;
	private String langCode;
	private String studentStatus;
}

