package com.ss.spa.model.fn;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)

public class Fnod02ReportModel extends BaseModel {
    // ----------required
    private String reportName;
    private String exportType;
    // ---------- use in Ireport
    private String companyCode;
    private String lang;
    private String[] receiveNo;
    private String docType;
    private String path;
}
