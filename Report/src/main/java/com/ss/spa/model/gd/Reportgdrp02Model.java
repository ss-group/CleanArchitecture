package com.ss.spa.model.gd;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)
public class Reportgdrp02Model extends BaseModel {
    // ----------required
    private String reportName;
    private String exportType;
    // ---------- use in Ireport 
    private String langCode;
    private String educationTypeLevel;
    private int registerYear;
    private int registerSemesterSeq;
    private int stuId;
    private String company;
    private String userName;

}