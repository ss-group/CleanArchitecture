package com.ss.spa.model;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)
public class Rp01TestEmpModel extends BaseModel {

    private String companyCode;
    private String reportName;
    private String exportType;

}
