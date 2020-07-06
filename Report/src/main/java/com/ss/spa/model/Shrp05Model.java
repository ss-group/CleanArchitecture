package com.ss.spa.model;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;
import java.util.Date;

@Data
@EqualsAndHashCode(callSuper=false)
public class Shrp05Model extends BaseModel {
    // ----------required
    private String reportName;
    private String exportType;
    // ---------- use in Ireport 
    //private String langCode;
    private String educationTypeLevel;
    private int registerYear;
    private String petitionCode;
    private String petitionCodeTo;
    private Date payDate;
    private Date payDateTo;
}