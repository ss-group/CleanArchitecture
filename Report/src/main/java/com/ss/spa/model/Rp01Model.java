package com.ss.spa.model;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)
public class Rp01Model extends BaseModel {

    private String tableName;
    private String reportName;
    private String exportType;

}
