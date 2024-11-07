﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIK_Test.Shared.Models;
public class Participant : IValidatableObject {

    public enum ParticipantType {
        Unkown = 0,
        Private = 1,
        Business = 2
    }
   
    public int Id { get; set; }

    public ParticipantType? Type { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public int? ParticipantCount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Description { get; set; }

    public List<Event> Events { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        if (string.IsNullOrWhiteSpace(Name)) yield return new ValidationResult("The name field is required");
        switch (Type) {
            case ParticipantType.Private:
                if (string.IsNullOrWhiteSpace(Code)) yield return new ValidationResult("The ID code field is required");
                if (!string.IsNullOrWhiteSpace(Code) && !IsValidIdCode(Code)) yield return new ValidationResult("The provided ID code is not valid");
                break;
            case ParticipantType.Business:
                break;
            default:
                yield return new ValidationResult("Type cannot be unkown");
                break; ;
        }
    }

    public bool IsValidIdCode(string idCode) {
        if (!IdCodeIsDigit(idCode)) return false;
        if (!IdCodeHasValidDate(idCode)) return false;
        if (!IdCodeHasValidControlNumber(idCode)) return false;
        return true;
    }

    public static bool IdCodeIsDigit(string idCode) {
        var charArr = idCode.ToCharArray();
        foreach (var item in charArr) {
            if (!Char.IsNumber(item)) return false;
        }
        return true;
    }

    public static bool IdCodeHasValidDate(string idCode) {
        var charArr = idCode.ToCharArray();

        int yearNr = (int)char.GetNumericValue(charArr[0]);
        string year;
        if (yearNr > 6) return false;
        switch (yearNr) {
            case 1:
            case 2:
                year = "18";
                break;
            case 3:
            case 4:
                year = "19";
                break;
            case 5:
            case 6:
                year = "20";
                break;
            default:
                return false;
        }
        year = year + charArr[1].ToString() + charArr[2].ToString();
        var month = charArr[3].ToString() + charArr[4].ToString();
        var day = charArr[5].ToString() + charArr[6].ToString();
        DateTime birthOfDate;
        try {
            birthOfDate = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
        }
        catch (ArgumentOutOfRangeException) {
            return false;
        }

        if (birthOfDate > DateTime.Now) return false;
        return true;
    }

    public static bool IdCodeHasValidControlNumber(string idCode) {
        int[] factors1 = [1, 2, 3, 4, 5, 6, 7, 8, 9, 1];
        int[] factors2 = [3, 4, 5, 6, 7, 8, 9, 1, 2, 3];
        var charArr = idCode.ToCharArray();
        var providedCtrlNr = (int)char.GetNumericValue(charArr[charArr.Length - 1]);
        int sum = 0;
        for (int i = 0; i < factors1.Length; i++) {
            sum += (int)char.GetNumericValue(charArr[i]) * factors1[i];
        }
        if ((sum % 11) == providedCtrlNr) return true;

        sum = 0;
        for (int j = 0; j < factors2.Length; j++) {
            sum += (int)char.GetNumericValue(charArr[j]) * factors2[j];
        }
        if ((sum % 11) == providedCtrlNr) return true;

        if (sum % 11 == 10 && providedCtrlNr == 0) return true;

        return false;
    }
}

