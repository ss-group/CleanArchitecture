package com.ss.spa.model;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)
public class Shrp03Model extends BaseModel {
    // ----------required
    private String reportName;
    private String exportType;
    // ---------- use in Ireport 
    //private String langCode;
    private String facCode;
    private String facCodeTo;
    private String programCode;
    private String programCodeTo;
    private String educationTypeLevel;
    private int registerYear;
}