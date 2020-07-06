package com.ss.spa.model.rg;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)
public class Rgrp02Model extends BaseModel {
	private int year;
	private int semester;
	private String educationTypeLevel;
	private String facCode;
	private String reportName;
	private String exportType;
	private String companyCode;
	private String langCode;
	private String publisher;
}
