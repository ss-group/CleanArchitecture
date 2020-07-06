package com.ss.spa.model;

import com.ss.spa.model.base.ExcelBaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper = false)
public class Rp01ExcelModel extends ExcelBaseModel {
	
	private String tableName;

}
