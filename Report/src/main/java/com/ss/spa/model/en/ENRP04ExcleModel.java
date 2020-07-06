package com.ss.spa.model.en;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;
import java.util.Date;

@Data
@EqualsAndHashCode(callSuper=false)
public class ENRP04ExcleModel extends BaseModel {
    // ----------required
    private String reportName;
    private String exportType;
	
    // ---------- use in Ireport 
	private int year;
	private int semester;
	private String educationTypeLevel;
	private long cclId;
	private String examineerIdFrom;
	private String examineerIdTo;
	private String taxId;
	private String lang;
	private String userName;
}
