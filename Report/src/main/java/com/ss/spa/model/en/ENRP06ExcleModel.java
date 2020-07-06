package com.ss.spa.model.en;

import com.ss.spa.model.base.BaseModel;
import lombok.Data;
import lombok.EqualsAndHashCode;
import java.util.Date;

@Data
@EqualsAndHashCode(callSuper=false)
public class ENRP06ExcleModel extends BaseModel {
    // ----------required
    private String reportName;
    private String exportType;
	
    // ---------- use in Ireport 
	private int year;
	private int semester;
	private String educationTypeLevel;
	private long cclId;
	private Date startDate;
	private Date toDate;
	private String lang;
	private String userName;
	private String companyCode;
}
