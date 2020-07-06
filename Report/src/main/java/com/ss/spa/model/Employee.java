package com.ss.spa.model;

import java.math.BigDecimal;
import java.util.Date;

import lombok.Data;

@Data
public class Employee {

	private String name;
	private Date birthDate;
	private BigDecimal payment;
	private BigDecimal bonus;
	private int age;
	
    public Employee(String name, Date birthDate, BigDecimal payment, BigDecimal bonus, int age) {
        this.name = name;
        this.birthDate = birthDate;
        this.payment = payment;
        this.bonus = bonus;
    }

    public Employee(String name, Date birthDate, double payment, double bonus, int age) {
        this(name, birthDate, new BigDecimal(payment), new BigDecimal(bonus), age);
    }
}
