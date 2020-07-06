package com.ss.spa.model.gd;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)
public class Reportgdrp01Model extends BaseModel {
    // ----------required
    private String reportName;
    private String exportType;
    // ---------- use in Ireport 
    private String langCode;
    private String educationTypeLevel;
    private int examYear;
    private int examSemesterSeq;
    private String subjCode;
    private String userName;

}