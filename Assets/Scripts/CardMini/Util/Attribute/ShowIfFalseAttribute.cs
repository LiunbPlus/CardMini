using UnityEngine;

public class ShowIfFalseAttribute : PropertyAttribute{
	public string boolFieldName;

	public ShowIfFalseAttribute(string boolFieldName){
		this.boolFieldName = boolFieldName;
	}
}

public class ShowIfTrueAttribute : PropertyAttribute{
	public string boolFieldName;

	public ShowIfTrueAttribute(string boolFieldName){
		this.boolFieldName = boolFieldName;
	}
}