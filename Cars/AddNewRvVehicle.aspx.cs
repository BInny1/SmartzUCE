﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CarsBL.Transactions;
using CarsInfo;
using System.Net.Mail;
using CarsBL.Masters;
using CarsBL.RvTransactions;
using CarsInfo.RvInfo;
using CarsBL.CentralDBTransactions;

public partial class AddNewRvVehicle : System.Web.UI.Page
{
    public GeneralFunc objGeneralFunc = new GeneralFunc();
    RvMainBL objRvMainBL = new RvMainBL();
    DropdownBL objdropdownBL = new DropdownBL();
    DataSet dsDropDown = new DataSet();
    RvCarsInfo objRvCarsInfo = new RvCarsInfo();
    RvSellerInfo objRvSellerInfo = new RvSellerInfo();
    RvUsedcarsInfo objRvUsedCarsInfo = new RvUsedcarsInfo();
    RvUserRegistrationInfo objUserRegInfo = new RvUserRegistrationInfo();
    DataSet dsActiveSaleAgents = new DataSet();
    CentralDBMainBL objCentralDBBL = new CentralDBMainBL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[Constants.NAME] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                lnkBtnLogout.Visible = true;
                lblUserName.Visible = true;
                string LogUsername = Session[Constants.NAME].ToString();
                if (LogUsername.Length > 10)
                {
                    lblUserName.Text = LogUsername.ToString().Substring(0, 10);
                }
                else
                {
                    lblUserName.Text = LogUsername;
                }
                if (Session["DsDropDown"] == null)
                {
                    dsDropDown = objdropdownBL.Usp_Get_DropDown();
                    Session["DsDropDown"] = dsDropDown;
                }
                else
                {
                    dsDropDown = (DataSet)Session["DsDropDown"];
                }
                dsActiveSaleAgents = objCentralDBBL.GetSmartzSalesAgentsActiveData();
                Session["ActiveSalesAgents"] = dsActiveSaleAgents;

                FillSaleDate();
                FillSaleAgents();
                FillVerifier();
                FillPaymentDate();
                FillPDDate();
                FillPackage();
                FillPayType();
                FillRegStates();
                FillYear();
                FillType();
                GetAllMakes();
                FillExteriorColor();
                FillInteriorColor();
                FillStates();
                FillCondition();
                FillFuelTypes();
                FillDoors();
                FillEngineManufacturer();
            }
        }
    }
    private void FillInteriorColor()
    {
        try
        {
            DataSet dsGetInteriorColors = new DataSet();
            if (Session["RvInteriorColors"] == null)
            {
                dsGetInteriorColors = objRvMainBL.GetInteriorColors();
                Session["RvInteriorColors"] = dsGetInteriorColors;
            }
            else
            {
                dsGetInteriorColors = Session["RvInteriorColors"] as DataSet;
            }
            ddlInteriorColor.DataSource = dsGetInteriorColors.Tables[0];
            ddlInteriorColor.DataTextField = "InteriorColorName";
            ddlInteriorColor.DataValueField = "InteriorColorName";
            ddlInteriorColor.DataBind();
            ddlInteriorColor.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
        }
        catch (Exception ex)
        {
        }
    }

    private void FillStates()
    {
        try
        {
            ddlLocationState.DataSource = dsDropDown.Tables[1];
            ddlLocationState.DataTextField = "State_Code";
            ddlLocationState.DataValueField = "State_ID";
            ddlLocationState.DataBind();
            ddlLocationState.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }
    private void FillExteriorColor()
    {
        try
        {
            DataSet dsGetExteriorColors = new DataSet();
            if (Session["RvExteriorColors"] == null)
            {
                dsGetExteriorColors = objRvMainBL.GetExteriorColors();
                Session["RvExteriorColors"] = dsGetExteriorColors;
            }
            else
            {
                dsGetExteriorColors = Session["RvExteriorColors"] as DataSet;
            }
            ddlExteriorColor.DataSource = dsGetExteriorColors.Tables[0];
            ddlExteriorColor.DataTextField = "ExteriorColorName";
            ddlExteriorColor.DataValueField = "ExteriorColorName";
            ddlExteriorColor.DataBind();
            ddlExteriorColor.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
        }
        catch (Exception ex)
        {
        }
    }
    private void FillType()
    {
        try
        {
            DataSet dsAllTypes = new DataSet();
            if (Session["RvAllTypes"] == null)
            {
                dsAllTypes = objRvMainBL.GetAllTypes();
                Session["RvAllTypes"] = dsAllTypes;
            }
            else
            {
                dsAllTypes = Session["RvAllTypes"] as DataSet;
            }

            ddlType.DataSource = dsAllTypes.Tables[0];
            ddlType.DataTextField = "TypeName";
            ddlType.DataValueField = "TypeID";
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }
    private void FillYear()
    {
        try
        {
            DataSet dsYears = new DataSet();
            if (Session["RvYears"] == null)
            {
                dsYears = objRvMainBL.GetYears();
                Session["RvYears"] = dsYears;
            }
            else
            {
                dsYears = Session["RvYears"] as DataSet;
            }
            ddlYear.DataSource = dsYears.Tables[0];
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }
    private void FillPayType()
    {
        try
        {
            DataSet dsPaymentMethod = new DataSet();
            if (Session["RvPayMethod"] == null)
            {
                dsPaymentMethod = objRvMainBL.GetPaymentMethod();
                Session["RvPayMethod"] = dsPaymentMethod;
            }
            else
            {
                dsPaymentMethod = Session["RvPayMethod"] as DataSet;
            }
            ddlPayMethod.DataSource = dsPaymentMethod.Tables[0];
            ddlPayMethod.DataTextField = "pmntType";
            ddlPayMethod.DataValueField = "pmntTypeID";
            ddlPayMethod.DataBind();
            //ddlPayMethod.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }
    private void FillPackage()
    {
        try
        {
            DataSet dsgetActivepackages = new DataSet();
            if (Session["RvPackages"] == null)
            {
                dsgetActivepackages = objRvMainBL.GetActivePackages();
                Session["RvPackages"] = dsgetActivepackages;
            }
            else
            {
                dsgetActivepackages = Session["RvPackages"] as DataSet;
            }
            for (int i = 0; i < dsgetActivepackages.Tables[0].Rows.Count; i++)
            {
                Double PackCost = new Double();
                PackCost = Convert.ToDouble(dsgetActivepackages.Tables[0].Rows[i]["Price"].ToString());
                string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
                string PackName = dsgetActivepackages.Tables[0].Rows[i]["Description"].ToString();
                ListItem list = new ListItem();
                list.Text = PackName + "($" + PackAmount + ")";
                list.Value = dsgetActivepackages.Tables[0].Rows[i]["PackageID"].ToString();
                ddlPackage.Items.Add(list);
            }
            ddlPackage.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GetAllModels()
    {
        try
        {
            DataSet dsAllModels = new DataSet();
            if (Session["RvAllModel"] == null)
            {
                dsAllModels = objRvMainBL.USP_GetAllModels(0);
                Session["RvAllModel"] = dsAllModels;
            }
            else
            {
                dsAllModels = Session["RvAllModel"] as DataSet;
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void FillCondition()
    {
        try
        {
            DataSet dsGetCondition = new DataSet();
            if (Session["RvCondition"] == null)
            {
                dsGetCondition = objRvMainBL.GetVehicleCondition();
                Session["RvCondition"] = dsGetCondition;
            }
            else
            {
                dsGetCondition = Session["RvCondition"] as DataSet;
            }
            ddlCondition.DataSource = dsGetCondition.Tables[0];
            ddlCondition.DataTextField = "condition";
            ddlCondition.DataValueField = "conditionID";
            ddlCondition.DataBind();
        }
        catch (Exception ex)
        {
        }
    }
    private void FillFuelTypes()
    {
        try
        {
            DataSet dsGetFuelTypes = new DataSet();
            if (Session["RvFuelTypes"] == null)
            {
                dsGetFuelTypes = objRvMainBL.GetFuelTypes();
                Session["RvFuelTypes"] = dsGetFuelTypes;
            }
            else
            {
                dsGetFuelTypes = Session["RvFuelTypes"] as DataSet;
            }
            ddlFuelType.DataSource = dsGetFuelTypes.Tables[0];
            ddlFuelType.DataTextField = "FuelType";
            ddlFuelType.DataValueField = "FuelTypeID";
            ddlFuelType.DataBind();
        }
        catch (Exception ex)
        {
        }
    }
    private void FillDoors()
    {
        try
        {
            DataSet dsGetAllDoors = new DataSet();
            if (Session["RvAllDoors"] == null)
            {
                dsGetAllDoors = objRvMainBL.GetAllDoors();
                Session["RvAllDoors"] = dsGetAllDoors;
            }
            else
            {
                dsGetAllDoors = Session["RvAllDoors"] as DataSet;
            }
            ddlDoorCount.DataSource = dsGetAllDoors.Tables[0];
            ddlDoorCount.DataTextField = "DoorsCount";
            ddlDoorCount.DataValueField = "DoorsCount";
            ddlDoorCount.DataBind();
            ddlDoorCount.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
        }
        catch (Exception ex)
        {
        }
    }
    private void FillEngineManufacturer()
    {
        try
        {
            DataSet dsGetEngineManufacturers = new DataSet();
            if (Session["RvEngineManufacturer"] == null)
            {
                dsGetEngineManufacturers = objRvMainBL.GetEngineManufacturer();
                Session["RvEngineManufacturer"] = dsGetEngineManufacturers;
            }
            else
            {
                dsGetEngineManufacturers = Session["RvEngineManufacturer"] as DataSet;
            }
            ddlEngineManufacturer.DataSource = dsGetEngineManufacturers.Tables[0];
            ddlEngineManufacturer.DataTextField = "EngineName";
            ddlEngineManufacturer.DataValueField = "EngineName";
            ddlEngineManufacturer.DataBind();
            ddlEngineManufacturer.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
        }
        catch (Exception ex)
        {
        }
    }


    private void GetAllMakes()
    {
        try
        {
            DataSet dsAllMakes = new DataSet();
            if (Session["RvAllMakes"] == null)
            {
                dsAllMakes = objRvMainBL.USP_GetAllMakes(0);
                Session["RvAllMakes"] = dsAllMakes;
            }
            else
            {
                dsAllMakes = Session["RvAllMakes"] as DataSet;
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void FillVerifier()
    {
        try
        {
            dsActiveSaleAgents = Session["ActiveSalesAgents"] as DataSet;
            ddlVerifier.DataSource = dsActiveSaleAgents.Tables[0];
            ddlVerifier.DataTextField = "American_Name";
            ddlVerifier.DataValueField = "Sale_Agent_Id";
            ddlVerifier.DataBind();
            ddlVerifier.Items.Insert(0, new ListItem("Unknown", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillSaleAgents()
    {
        try
        {
            dsActiveSaleAgents = Session["ActiveSalesAgents"] as DataSet;
            ddlSalesAgent.DataSource = dsActiveSaleAgents.Tables[0];
            ddlSalesAgent.DataTextField = "American_Name";
            ddlSalesAgent.DataValueField = "Sale_Agent_Id";
            ddlSalesAgent.DataBind();
            ddlSalesAgent.Items.Insert(0, new ListItem("Unknown", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    private void FillPDDate()
    {
        try
        {
            for (int i = 0; i < 21; i++)
            {
                ListItem list = new ListItem();
                list.Text = System.DateTime.Now.ToUniversalTime().AddHours(-4).AddDays(i).ToString("MM/dd/yyyy");
                list.Value = System.DateTime.Now.ToUniversalTime().AddHours(-4).AddDays(i).ToString("MM/dd/yyyy");
                ddlPDDate.Items.Add(list);
            }
            ddlPDDate.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillRegStates()
    {
        try
        {
            ddlRegState.DataSource = dsDropDown.Tables[1];
            ddlRegState.DataTextField = "State_Code";
            ddlRegState.DataValueField = "State_ID";
            ddlRegState.DataBind();
            ddlRegState.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    private void FillPaymentDate()
    {
        try
        {
            for (int i = 0; i < 7; i++)
            {
                ListItem list = new ListItem();
                list.Text = System.DateTime.Now.ToUniversalTime().AddHours(-4).AddDays(-i).ToString("MM/dd/yyyy");
                list.Value = System.DateTime.Now.ToUniversalTime().AddHours(-4).AddDays(-i).ToString("MM/dd/yyyy");
                ddlPaymentDate.Items.Add(list);
            }
            ddlPaymentDate.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillSaleDate()
    {
        try
        {
            for (int i = 0; i < 7; i++)
            {
                ListItem list = new ListItem();
                list.Text = System.DateTime.Now.ToUniversalTime().AddHours(-4).AddDays(-i).ToString("MM/dd/yyyy");
                list.Value = System.DateTime.Now.ToUniversalTime().AddHours(-4).AddDays(-i).ToString("MM/dd/yyyy");
                ddlSaleDate.Items.Add(list);
            }
            ddlSaleDate.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetModelsInfo();
        }
        catch (Exception ex)
        {
        }
    }
    public void GetModelsInfo()
    {
        try
        {
            DataSet dsMakes = Session["RvAllMakes"] as DataSet;
            int RVTYPE = Convert.ToInt32(ddlType.SelectedItem.Value);
            DataView dvModel = new DataView();
            DataTable dtModel = new DataTable();
            dvModel = dsMakes.Tables[0].DefaultView;
            dvModel.RowFilter = "RVTYPE='" + RVTYPE.ToString() + "'";
            dtModel = dvModel.ToTable();
            ddlMake.DataSource = dtModel;
            ddlMake.Items.Clear();
            ddlMake.DataTextField = "make";
            ddlMake.DataValueField = "makeID";
            ddlMake.DataBind();
            ddlMake.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }
    protected void ImgBtnLogo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("Index.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkbtnHome_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("index.aspx");
        }
        catch (Exception ex)
        {
        }
    }
    protected void lnkbtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("SearchNew.aspx");
        }
        catch (Exception ex)
        {
        }
    }
    protected void lnkBtnLogout_Click(object sender, EventArgs e)
    {
        try
        {
            CentralDBMainBL objCentralDBBL = new CentralDBMainBL();
            objCentralDBBL.Perform_LogOut(Convert.ToInt32(Session[Constants.USER_ID]), System.DateTime.Now, Convert.ToInt32(Session[Constants.USERLOG_ID]), 2);
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("NewCustomer.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            objUserRegInfo.Name = objGeneralFunc.ToProper(txtRegName.Text).Trim();
            objUserRegInfo.UserName = txtLoginEmail.Text;
            objUserRegInfo.Pwd = txtLoginPassword.Text;
            objUserRegInfo.PhoneNumber = txtRegPhNo.Text;
            objUserRegInfo.CouponCode = "";
            objUserRegInfo.ReferCode = "";
            objUserRegInfo.Address = objGeneralFunc.ToProper(txtRegAddress.Text).Trim();
            objUserRegInfo.City = objGeneralFunc.ToProper(txtRegCity.Text).Trim();
            objUserRegInfo.StateID = Convert.ToInt32(ddlRegState.SelectedItem.Value);
            objUserRegInfo.AltEmail = txtRegAltEmail.Text;
            objUserRegInfo.IsActive = 1;
            objUserRegInfo.Zip = txtregZip.Text;
            objUserRegInfo.BusinessName = txtRegBusinessName.Text;
            objUserRegInfo.AltPhone = txtRegAltPhoneNum.Text;
            objUserRegInfo.SaleAgentID = Convert.ToInt32(ddlSalesAgent.SelectedItem.Value);
            objUserRegInfo.VerifierID = Convert.ToInt32(ddlVerifier.SelectedItem.Value);

            int PackageID = Convert.ToInt32(ddlPackage.SelectedItem.Value);
            objUserRegInfo.PackageID = Convert.ToInt32(ddlPackage.SelectedItem.Value.ToString());
            DataSet dsUserExists = objRvMainBL.ChkUserExists(txtLoginEmail.Text);
            if (dsUserExists.Tables.Count > 0)
            {
                if (dsUserExists.Tables[0].Rows.Count > 0)
                {
                    Session.Timeout = 180;
                    mdepAlertExists.Show();
                    lblErrorExists.Visible = true;
                    lblErrorExists.Text = "Email " + txtLoginEmail.Text + " already exists";
                }
                else
                {
                    DataSet dsUserInfo = objRvMainBL.SmartzSave_RegisterLogUser(objUserRegInfo);
                    Session["RegRvUSER_ID"] = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UId"].ToString());
                    Session["RegRvUserName"] = dsUserInfo.Tables[0].Rows[0]["UserName"].ToString();
                    Session["RVRegName"] = dsUserInfo.Tables[0].Rows[0]["Name"].ToString();
                    Session["RegRvPhoneNumber"] = dsUserInfo.Tables[0].Rows[0]["PhoneNumber"].ToString();
                    Session["RvPackageID"] = dsUserInfo.Tables[0].Rows[0]["PackageID"].ToString();
                    Session["RegRvPassword"] = dsUserInfo.Tables[0].Rows[0]["Pwd"].ToString();
                    Session["RegRvUserPackID"] = dsUserInfo.Tables[0].Rows[0]["UserPackID"].ToString();
                    SaveData();
                    Session.Timeout = 180;
                    mpealteruserUpdated.Show();
                    lblErrUpdated.Visible = true;
                    lblErrUpdated.Text = "Customer details saved successfully with RVID " + Session["RvCarID"].ToString();
                }
            }
            else
            {
                DataSet dsUserInfo = objRvMainBL.SmartzSave_RegisterLogUser(objUserRegInfo);
                Session["RegRvUSER_ID"] = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UId"].ToString());
                Session["RegRvUserName"] = dsUserInfo.Tables[0].Rows[0]["UserName"].ToString();
                Session["RVRegName"] = dsUserInfo.Tables[0].Rows[0]["Name"].ToString();
                Session["RegRvPhoneNumber"] = dsUserInfo.Tables[0].Rows[0]["PhoneNumber"].ToString();
                Session["RvPackageID"] = dsUserInfo.Tables[0].Rows[0]["PackageID"].ToString();
                Session["RegRvPassword"] = dsUserInfo.Tables[0].Rows[0]["Pwd"].ToString();
                Session["RegRvUserPackID"] = dsUserInfo.Tables[0].Rows[0]["UserPackID"].ToString();
                SaveData();
                Session.Timeout = 180;
                mpealteruserUpdated.Show();
                lblErrUpdated.Visible = true;
                lblErrUpdated.Text = "Customer details saved successfully with RVID " + Session["RvCarID"].ToString();
            }
            //SaveData();


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void SaveData()
    {
        try
        {
            objRvCarsInfo.YearOfMake = Convert.ToInt32(ddlYear.SelectedItem.Text);
            objRvCarsInfo.MakeModelID = Convert.ToInt32(ddlMake.SelectedItem.Value);
            Session["RvSelYear"] = ddlYear.SelectedItem.Text;
            Session["RvSelMake"] = ddlMake.SelectedItem.Text;
            Session["RvSelType"] = ddlType.SelectedItem.Text;
            objRvCarsInfo.Model = objGeneralFunc.ToProper(txtModel.Text);

            if (txtAskingPrice.Text == "")
            {
                objRvCarsInfo.Price = "0";
            }
            else
            {
                objRvCarsInfo.Price = txtAskingPrice.Text;
            }
            if (txtMileage.Text == "")
            {
                objRvCarsInfo.Mileage = "0";
            }
            else
            {
                objRvCarsInfo.Mileage = txtMileage.Text;
            }
            objRvCarsInfo.ExteriorColor = ddlExteriorColor.SelectedItem.Text;
            objRvCarsInfo.InteriorColor = ddlInteriorColor.SelectedItem.Text;
            objRvCarsInfo.NumberOfDoors = ddlDoorCount.SelectedItem.Value;
            objRvCarsInfo.VIN = txtVinNumber.Text;
            objRvCarsInfo.FuelTypeID = Convert.ToInt32(ddlFuelType.SelectedItem.Value);
            objRvCarsInfo.Description = txtDescription.Text;
            objRvCarsInfo.ConditionDescription = ddlCondition.SelectedItem.Text;
            objRvCarsInfo.Title = txtTitle.Text;
            objRvCarsInfo.AirConditioners = Convert.ToInt32(ddlAirConditioners.SelectedItem.Value);
            objRvCarsInfo.Length = txtLength.Text;
            objRvCarsInfo.WaterCapacity = txtWaterCapacity.Text;
            objRvCarsInfo.Towing_Capacity = txtTowingCapacity.Text;
            objRvCarsInfo.Dry_Weight = txtDryWeight.Text;
            objRvCarsInfo.SleepingCapacity = Convert.ToInt32(ddlSleepingCapacity.SelectedItem.Value);
            objRvCarsInfo.SlideOuts = Convert.ToInt32(ddlSlideOuts.SelectedItem.Value);
            objRvCarsInfo.EngineManufacturer = ddlEngineManufacturer.SelectedItem.Text;
            objRvCarsInfo.EngineType = txtEngineModel.Text;
            objRvCarsInfo.VehicleConditionID = Convert.ToInt32(ddlCondition.SelectedItem.Value);
            objRvCarsInfo.StateID = Convert.ToInt32(ddlLocationState.SelectedItem.Value);
            objRvSellerInfo.SellerName = "";
            objRvSellerInfo.Address1 = "";
            objRvSellerInfo.Phone = txtCustPhoneNumber.Text;
            objRvSellerInfo.AltPhone = txtCustAltNumber.Text;
            objRvSellerInfo.State = ddlLocationState.SelectedItem.Text;
            objRvSellerInfo.Email = txtSellerEmail.Text;
            objRvSellerInfo.State = ddlLocationState.SelectedItem.Text;
            objRvSellerInfo.City = objGeneralFunc.ToProper(txtCity.Text);
            objRvSellerInfo.Zip = txtZip.Text;
            Session["RvSellerZip"] = txtZip.Text;
            String strHostName = Request.UserHostAddress.ToString();
            string strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            objRvUsedCarsInfo.Ip = strIp;
            objRvUsedCarsInfo.Uid = Convert.ToInt32(Session["RegRvUSER_ID"].ToString());
            objRvUsedCarsInfo.SaleDate = Convert.ToDateTime(ddlSaleDate.SelectedItem.Text);
            objRvUsedCarsInfo.PackageID = Convert.ToInt32(ddlPackage.SelectedItem.Value);
            objRvUsedCarsInfo.SaleEnteredBy = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            objRvUsedCarsInfo.Carid = Convert.ToInt32(0);

            int PackageID = Convert.ToInt32(ddlPackage.SelectedItem.Value);

            DataSet dsTotalData = objRvMainBL.SmartzSaveRVData(objRvUsedCarsInfo, objRvCarsInfo, objRvSellerInfo);
            Session["RvCarID"] = dsTotalData.Tables[0].Rows[0]["CarID"].ToString();
            Session["RvPostingID"] = dsTotalData.Tables[0].Rows[0]["PostingID"].ToString();
            Session["RvUniqueID"] = dsTotalData.Tables[0].Rows[0]["CarUniqueID"].ToString();



            int LoginID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            DataSet dsCarFeature = new DataSet();
            int CarIDs;
            int Isactive = 0;
            int FeatureID;
            for (int i = 1; i < 4; i++)
            {
                CarIDs = Convert.ToInt32(Session["RvCarID"].ToString());
                string ChkBoxID = "chkFeatures" + i.ToString();
                CheckBox ChkedBox = (CheckBox)form1.FindControl(ChkBoxID);
                if (ChkedBox.Checked == true)
                {
                    Isactive = 1;
                }
                else
                {
                    Isactive = 0;
                }
                FeatureID = i;
                dsCarFeature = objRvMainBL.SmartzSaveRvFeatures(CarIDs, FeatureID, Isactive, LoginID);

            }
            Session["CarID"] = Convert.ToInt32(dsCarFeature.Tables[0].Rows[0]["CarID"].ToString());



            int PmntStatus = Convert.ToInt32(ddlPayStatus.SelectedItem.Value);
            Session["NewUserPayStatus"] = PmntStatus;
            Session["NewUserPDDate"] = 0;
            int PmntType;
            string TransactionID;
            int AdActive;
            string PayAmount;
            int ListingStatus;
            DateTime PDDate;
            int UserPackID = Convert.ToInt32(Session["RegRvUserPackID"].ToString());
            int PostingID = Convert.ToInt32(Session["RvPostingID"].ToString());
            int RegUID = Convert.ToInt32(Session["RegRvUSER_ID"].ToString());
            string VoiceFileName = txtVoiceFileName.Text;
            if (PmntStatus == 1)
            {
                PmntType = 0;
                TransactionID = "";
                AdActive = 0;
                PayAmount = "";
            }
            else
            {
                if (PmntStatus == 5)
                {
                    PmntType = Convert.ToInt32(ddlPayMethod.SelectedItem.Value);

                    TransactionID = "";
                    AdActive = Convert.ToInt32(ddlAdStatus.SelectedItem.Value);
                    PayAmount = txtPayAmount.Text;
                }
                else
                {
                    PmntType = Convert.ToInt32(ddlPayMethod.SelectedItem.Value);
                    TransactionID = txtPayConfirmNum.Text;
                    AdActive = Convert.ToInt32(ddlAdStatus.SelectedItem.Value);
                    PayAmount = txtPayAmount.Text;
                }
            }
            if (AdActive == 0)
            {
                ListingStatus = Convert.ToInt32(ddlListingStatus.SelectedItem.Value);
            }
            else
            {
                ListingStatus = 1;
            }
            if (PackageID != 1)
            {
                DateTime Paymentdate;
                if (PmntStatus == 5)
                {
                    PDDate = Convert.ToDateTime(ddlPDDate.SelectedItem.Text);
                    Paymentdate = Convert.ToDateTime(PDDate);
                }
                else if (PmntStatus == 1)
                {
                    Paymentdate = Convert.ToDateTime(ddlSaleDate.SelectedItem.Text);
                    PDDate = Convert.ToDateTime(Paymentdate);
                }
                else
                {
                    Paymentdate = Convert.ToDateTime(ddlPaymentDate.SelectedItem.Text);
                    PDDate = Convert.ToDateTime(Paymentdate);
                }
                Session["NewUserPDDate"] = PDDate;
                bool bnewPay = objRvMainBL.SmartzSaveRvPmntDetails(PmntType, PmntStatus, TransactionID, strIp, RegUID, AdActive, PayAmount, Paymentdate, ListingStatus, PDDate, UserPackID, PostingID, VoiceFileName);
            }
            else
            {
                bool bnewPay = objRvMainBL.SmartzSaveRvPmntDetailsForFreePackage(RegUID, AdActive, ListingStatus, UserPackID, PostingID);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void btnYesUpdated_Click(object sender, EventArgs e)
    {
        try
        {
            ResendRegMail();
            if (Session["RvSellerZip"].ToString() != "")
            {
                string SellerZip = Session["RvSellerZip"].ToString();
                DataSet dsZipExists = objRvMainBL.SmartzCheckZipExists(SellerZip);
                if (dsZipExists.Tables[0].Rows[0]["Result"].ToString() == "Yes")
                {
                    Response.Redirect("Index.aspx");
                }
                else
                {
                    int CarID = Convert.ToInt32(Session["CarID"].ToString());
                    int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
                    int CallType = Convert.ToInt32(8);
                    int CallReason = Convert.ToInt32(4);
                    int CallResolution = Convert.ToInt32(8);
                    string SpokeWith = "Internal Ticket";
                    string Notes = "Internal ticket for zip " + SellerZip.ToString() + " is not exists";
                    int TicketType = Convert.ToInt32(3);
                    int Priority = Convert.ToInt32(2);
                    int CallBackID = Convert.ToInt32(1);
                    string TicketDescription = "Internal ticket for zip " + SellerZip.ToString() + " is not exists";
                    bool bnew = objRvMainBL.USP_SmartzSaveCSandTicketDetails(CarID, UID, CallType, CallReason, Notes, TicketType, Priority, CallBackID, TicketDescription, CallResolution, SpokeWith);
                    if (bnew == true)
                    {
                        Response.Redirect("Index.aspx");
                    }
                    else
                    {
                        Response.Redirect("Index.aspx");
                    }
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BtnClsUpdated_Click(object sender, EventArgs e)
    {
        try
        {
            ResendRegMail();
            if (Session["RvSellerZip"].ToString() != "")
            {
                string SellerZip = Session["RvSellerZip"].ToString();
                DataSet dsZipExists = objRvMainBL.SmartzCheckZipExists(SellerZip);
                if (dsZipExists.Tables[0].Rows[0]["Result"].ToString() == "Yes")
                {
                    Response.Redirect("Index.aspx");
                }
                else
                {
                    int CarID = Convert.ToInt32(Session["CarID"].ToString());
                    int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
                    int CallType = Convert.ToInt32(8);
                    int CallReason = Convert.ToInt32(4);
                    int CallResolution = Convert.ToInt32(8);
                    string SpokeWith = "Internal Ticket";
                    string Notes = "Internal ticket for zip " + SellerZip.ToString() + " is not exists";
                    int TicketType = Convert.ToInt32(3);
                    int Priority = Convert.ToInt32(2);
                    int CallBackID = Convert.ToInt32(1);
                    string TicketDescription = "Internal ticket for zip " + SellerZip.ToString() + " is not exists";
                    bool bnew = objRvMainBL.USP_SmartzSaveCSandTicketDetails(CarID, UID, CallType, CallReason, Notes, TicketType, Priority, CallBackID, TicketDescription, CallResolution, SpokeWith);
                    if (bnew == true)
                    {
                        Response.Redirect("Index.aspx");
                    }
                    else
                    {
                        Response.Redirect("Index.aspx");
                    }
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void ResendRegMail()
    {
        try
        {
            string PDDate = string.Empty;
            string LoginPassword = Session["RegRVPassword"].ToString();
            string LoginName = Session["RegRVUserName"].ToString();
            string UserDisName = Session["RVRegName"].ToString();

            string RvYear = Session["RvSelYear"].ToString();
            string RvType = Session["RvSelType"].ToString();
            string RvMake = Session["RvSelMake"].ToString();
            string RvUniqueID = Session["RvUniqueID"].ToString();
            RvType = RvType.Replace(" ", "%20");
            RvMake = RvMake.Replace(" ", "%20");
            string Link = "http://unitedrvexchange.com/Buy-Sell-UsedRVs/" + RvYear + "-" + RvType + "-" + RvMake + "-" + RvUniqueID;
            string TermsLink = "http://unitedrvexchange.com/TermsandConditions.aspx";
            clsMailFormats format = new clsMailFormats();
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("info@unitedrvexchange.com");
            msg.To.Add(LoginName);
            msg.Bcc.Add("archive@unitedrvexchange.com");
            msg.Subject = "Registration Details From United Rv Exchange For RV ID# " + Session["RvCarID"].ToString();
            msg.IsBodyHtml = true;
            string text = string.Empty;
            if (Session["NewUserPayStatus"].ToString() == "5")
            {
                DateTime PostDate = Convert.ToDateTime(Session["NewUserPDDate"].ToString());
                PDDate = PostDate.ToString("MM/dd/yyyy");
                text = format.SendRVRegistrationdetailsForPDSales(LoginName, LoginPassword, UserDisName, ref text, PDDate);
            }
            else
            {
                text = format.SendRVRegistrationdetails(LoginName, LoginPassword, UserDisName, ref text, Link, TermsLink);
            }
            msg.Body = text.ToString();
            SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 587;
            //smtp.Credentials = new System.Net.NetworkCredential("info@unitedcarexchange.com", "info*123*");
            //smtp.EnableSsl = true;
            //smtp.Send(msg);
            smtp.Host = "127.0.0.1";
            smtp.Port = 25;
            smtp.Send(msg);
        }
        catch (Exception ex)
        {
            //throw ex;
            Response.Redirect("EmailServerError.aspx");
        }
    }

    protected void ddlPayStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int PayStatus = Convert.ToInt32(ddlPayStatus.SelectedItem.Value);
            if (PayStatus == 1)
            {
                tblPaymentDetails.Style["display"] = "none";
                trListingStatus.Style["display"] = "none";
            }
            else
            {

                tblPaymentDetails.Style["display"] = "block";
                if (PayStatus == 5)
                {
                    trPDDate.Style["display"] = "block";
                    trPayDate.Style["display"] = "none";
                    trPayConfirm.Style["display"] = "none";
                    tblVoiceFile.Style["display"] = "none";
                }
                else
                {
                    trPDDate.Style["display"] = "none";
                    trPayDate.Style["display"] = "block";
                    trPayConfirm.Style["display"] = "block";
                    tblVoiceFile.Style["display"] = "block";
                }
                int AdStatus = Convert.ToInt32(ddlAdStatus.SelectedItem.Value);
                if (AdStatus == 1)
                {
                    trListingStatus.Style["display"] = "none";
                }
                else
                {
                    trListingStatus.Style["display"] = "block";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlAdStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int AdStatus = Convert.ToInt32(ddlAdStatus.SelectedItem.Value);
            if (AdStatus == 1)
            {
                trListingStatus.Style["display"] = "none";
            }
            else
            {
                trListingStatus.Style["display"] = "block";

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
