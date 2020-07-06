package com.ss.spa.model.fn;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)
public class FNRP01Model extends BaseModel {
    // ----------required
    private String reportName;
    private String exportType;
    // ---------- use in Ireport
    private String companyCode;
    private String lang;
    private String userName;
    private String systemCode;
    private int year;
    private int semester;
    private String educationLv;
    private String educationLvTo;
    private String facCode;
    private String facCodeTo;
    private String proCode;
    private String proCodeTo;
    private String pricelistCode;
    private String pricelistCodeTo;
    private String regTypeCode;
    private String regTypeCodeTo;
    private String petitionCode;
    private String petitionCodeTo;
    
}
