package com.ss.spa.core.utils;

import javax.validation.constraints.NotNull;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.validation.annotation.Validated;
import org.springframework.core.io.Resource;
import lombok.Data;

@Data
@Validated
@ConfigurationProperties(prefix = "com.ss.spa")
public class ApplicationProperties {
	
	/**
	 * The base path where reports will be stored after compilation
	 */
	@NotNull
	private Resource storageLocation;
	/**
	 * The location of JasperReports source files
	 */
	@NotNull
	private Resource reportLocation;

}
