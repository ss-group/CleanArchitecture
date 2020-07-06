package com.ss.spa.model;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;

@Data
@EqualsAndHashCode(callSuper=false)
public class Gsrp02Model extends BaseModel {
	private String userName;
	private String lang;
	private int year;
	private int period;
	private String eduTypeLevel;
	private String fromProgram;
	private String toProgram;
	private String toFac;
	private String fromFac;
	private String fromStudentCode;
	private String toStudentCode;
	private String type;
	private String exportType;
	private String company;
}
