//----------------------------------------------------------------------------
// Copyright (C) 2011, AGRICULTURAL BANK OF CHINA, Corp. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using System.Data;
using Model;
using System.IO;
using System.Xml;

namespace DAL
{
    /// <summary>
    /// 系统init文件
    /// </summary>
    /// ----------------------------------------------------------
    ///修改历史
    ///日期       修改人      修改
    ///----------------------------------------------------------
    ///20110301    王强      创建代码
    ///----------------------------------------------------------
    public static class init
    {
        /// <summary>
        /// Hashtable
        /// </summary>
        private static Hashtable glbhs = null;
        /// <summary>
        /// Hashtable
        /// </summary>
        private static Hashtable authcnamhs = null;
        /// <summary>
        /// Hashtable
        /// </summary>
        private static Hashtable authhs = null;
        /// <summary>
        /// Hashtable
        /// </summary>
        private static Hashtable datalimths = null;
        /// <summary>
        /// Hashtable
        /// </summary>
        private static Hashtable errnohs = null;
        /// <summary>
        /// Hashtable
        /// </summary>
        private static NoSortHashtable rolnamhs = null;
        /// <summary>
        /// Hashtable
        /// </summary>
        private static Hashtable rollevhs = null;
        /// <summary>
        /// Hashtable
        /// </summary>
        private static Hashtable scuhs = null;
        /// <summary>
        /// Hashtable
        /// </summary>
        private static NoSortHashtable titlehs = null;

        /// <summary>
        /// 状态表
        /// </summary>
        private static Hashtable activehs = null;

        /// <summary>
        /// Hashtable
        /// </summary>
        private static Hashtable statushs = null;

        private static Hashtable mtnumhs = null;

        private static Hashtable systype = null;
        //文档类型
        private static Hashtable doctype = null;

        //流程图对应的
        public static Hashtable workFlow = null;
        //信息提示相关
        public static Hashtable promptInfo = null;

        /// <summary>
        /// 可见范围相关
        /// </summary>
        private static Hashtable scopehs = null;

        /// <summary>
        /// 提取配置信息
        /// </summary>
        static init()
        {

            /////////////////////使用的全局变量///////////////////////////////
            glbhs = new Hashtable();
            glbhs["mindate"] = "19000101";
            glbhs["maxdate"] = "99991231";

            /////////////////////使用的全局变量///////////////////////////////

            /////////////////////wq 信息提示相关///////////////////////////////
            promptInfo = new Hashtable();
            promptInfo["00"] = "任务单受理人";
            promptInfo["01"] = "任务单任务经理";
            promptInfo["10"] = "应用系统研发部门联系人";
            promptInfo["11"] = "应用系统业务部门联系人";
            promptInfo["12"] = "应用系统1线A角";
            promptInfo["13"] = "应用系统1线B角";
            promptInfo["14"] = "应用系统2线A角";
            promptInfo["15"] = "应用系统2线B角";
            promptInfo["16"] = "应用系统3线A角";
            promptInfo["17"] = "应用系统3线B角";
            promptInfo["pre"] = "之前为";
            promptInfo["post"] = "";
            promptInfo["flag_0"] = "未处理";
            promptInfo["flag_1"] = "已处理";
            promptInfo["flag_9"] = "已关闭";
            /////////////////////wq 信息提示相关///////////////////////////////
            


            ////////////////wq任务单流程图相关////////////////////////////////
            workFlow = new Hashtable();
            //流程名称
            workFlow["flow0"] = "丁利流程";
            workFlow["flow1"] = "龚毅流程";

            //显示状态对应显示到后台的含义名称
            workFlow["0_name"] = "创建";
            workFlow["1_name"] = "本步完成,待顶层任务经理确认";
            workFlow["2_name"] = "本步完成,下步任务可开始";
            workFlow["3_name"] = "待任务经理分配";
            workFlow["4_name"] = "待受理人受理";
            workFlow["5_name"] = "处理中";
            workFlow["6_name"] = "完成待任务经理确认";
            workFlow["7_name"] = "已完成";
            workFlow["8_name"] = "关闭待确认";
            workFlow["9_name"] = "已关闭";
            workFlow["o_name"] = "任务原来的状态";
            workFlow["C_name"] = "变更已完成";


            //动作码对应中文名称
            workFlow["CJRW"] = "创建任务";
            workFlow["FPRW"] = "分配任务";
            workFlow["GBRW"] = "关闭任务";
            workFlow["HTRW"] = "回退任务";
            workFlow["SLRW"] = "受理任务";
            workFlow["WCRW"] = "完成任务";
            workFlow["ZFP"] = "再分配";
            workFlow["QRWC"] = "确认完成";
            workFlow["QRWWC"] = "确认未完成";
            workFlow["TGSH"] = "通过审核";
            workFlow["WTGSH"] = "未通过审核";
            workFlow["JXWC"] = "继续完成";
            workFlow["CQRW"] = "重启任务";
            workFlow["YSTG"] = "预审通过";
            workFlow["SYSCOMPUTE"] = "系统计算";


            //条件码对应中文名称
            workFlow["mng==crt"] = "任务经理与创建人为同一人";
            workFlow["!mng==crt"] = "任务经理与创建人不是同一人";
            workFlow["mng==asn"] = "任务经理与受理人是同一人";
            workFlow["!mng==asn"] = "任务经理与受理人不是同一人";
            workFlow["mng==topmng"] = "任务经理与顶层任务经理是同一人";
            workFlow["!mng==topmng"] = "任务经理与顶层任务经理不是同一人";
            workFlow["HaveNextStep"] = "有下一层任务";
            workFlow["!HaveNextStep"] = "没有下一层任务";
            workFlow["AllCldComplete"] = "所有下一层任务均已完成";
            workFlow["!AllCldComplete"] = "下一层有任务未完成";
            workFlow["AllPrtComplete"] = "所有上层任务均完成";
            workFlow["!AllPrtComplete"] = "上层有任务未完成";
            workFlow["AllBrdComplete"] = "所有兄弟任务均完成";
            workFlow["!AllBrdComplete"] = "兄弟有任务未完成";
            workFlow["IsPrtIn5"] = "上层任务处理中";
            workFlow["!IsPrtIn5"] = "上层任务未在处理中";
            workFlow["IsCldIn0"] = "下层任务处与创建状态";
            workFlow["!IsCldIn0"] = "下层任务不处于创建状态";
            workFlow["IsTopTask"] = "是顶层任务";
            workFlow["!IsTopTask"] = "非顶层任务";
            workFlow["IsSTask"] = "是串行任务";
            workFlow["!IsSTask"] = "非串行任务";

            //某流程某状态经过某一动作，某条件，流转的下一个状态
            //////////////////////////////////////////流程0（丁利流程）//////////////////////////////////////
            ///////////////创建状态/////////////////////////
            //创建状态流转
            workFlow["flow0_0_CJRW_!cat==21&mng==asn"] = "5";
            workFlow["flow0_0_CJRW_!cat==21&!mng==topmng&!mng==asn"] = "3";
            workFlow["flow0_0_CJRW_!cat==21&IsTopTask&!mng==asn"] = "3";
            workFlow["flow0_0_CJRW_!cat==21&!IsTopTask&mng==topmng&!mng==asn"] = "4";
            workFlow["flow0_0_CJRW_cat==21"] = "5";
            workFlow["flow0_0_CJRW_AllCondition"] = "!cat==21&mng==asn,!cat==21&!mng==topmng&!mng==asn,!cat==21&IsTopTask&!mng==asn,!cat==21&!IsTopTask&mng==topmng&!mng==asn,cat==21";
            workFlow["flow0_0_ZFP_!acpt==asn&crtusr==mng"] = "0";
            workFlow["flow0_0_ZFP_!acpt==asn&!crtusr==mng"] = "0";
            workFlow["flow0_0_ZFP_AllCondition"] = "!acpt==asn&crtusr==mng,!acpt==asn&!crtusr==mng";
            workFlow["flow0_0_WCRW"] = "3";
            workFlow["flow0_0_WCRW_AllCondition"] = "";
            workFlow["flow0_0_GBRW_crtusr==mng"] = "9";
            workFlow["flow0_0_GBRW_crtusr==creater"] = "9";
            workFlow["flow0_0_GBRW_AllCondition"] = "crtusr==mng,crtusr==creater";
            workFlow["flow0_0_CQRW_crtusr==mng"] = "o";
            workFlow["flow0_0_CQRW_crtusr==creater"] = "o";
            workFlow["flow0_0_CQRW_AllCondition"] = "crtusr==mng,crtusr==creater";
            //创建状态流转
            //创建执行的op
            workFlow["flow0_0_CJRW_!cat==21&mng==asn_op"] = "UpdState,Log,AddCost";
            workFlow["flow0_0_CJRW_!cat==21&!mng==topmng&!mng==asn_op"] = "UpdState";
            workFlow["flow0_0_CJRW_!cat==21&IsTopTask&!mng==asn_op"] = "UpdState";
            workFlow["flow0_0_CJRW_!cat==21&!IsTopTask&mng==topmng&!mng==asn_op"] = "UpdState,Log";
            workFlow["flow0_0_CJRW_cat==21_op"] = "UpdState";
            workFlow["flow0_0_ZFP_!acpt==asn&crtusr==mng_op"] = "UpdAsn";
            workFlow["flow0_0_ZFP_!acpt==asn&!crtusr==mng_op"] = "UpdAsn";
            workFlow["flow0_0_WCRW_op"] = "UpdState";
            workFlow["flow0_0_GBRW_crtusr==mng_op"] = "UpdState,Log,ChildStart";
            workFlow["flow0_0_GBRW_crtusr==creater_op"] = "UpdState,Log,ChildStart";
            workFlow["flow0_0_CQRW_crtusr==mng_op"] = "UpdState,Log";
            workFlow["flow0_0_CQRW_crtusr==creater_op"] = "UpdState,Log";
            //op的参数
            workFlow["flow0_0_CJRW_!cat==21&mng==asn_Log"] = "&mng&受理任务单";
            workFlow["flow0_0_CJRW_!cat==21&!IsTopTask&mng==topmng&!mng==asn_Log"] = "&mng&分配任务单";
            workFlow["flow0_0_GBRW_crtusr==mng_Log"] = "&mng&关闭任务单&reason&";
            workFlow["flow0_0_GBRW_crtusr==creater_Log"] = "&mng&关闭任务单&reason&";
            workFlow["flow0_0_CQRW_crtusr==mng_Log"] = "&crtusr&重启任务单";
            workFlow["flow0_0_CQRW_crtusr==creater_Log"] = "&crtusr&重启任务单";


            ////////////////创建状态//////////////////////

            //////////////////////待分配状态////////////////////////////////
            //待分配状态流转
            workFlow["flow0_3_FPRW_IsAcptEmpty"] = "3";
            workFlow["flow0_3_FPRW_acpt==crtusr"] = "5";
            workFlow["flow0_3_FPRW_acpt==asn&mng==acpt"] = "5";
            workFlow["flow0_3_FPRW_!acpt==asn&mng==acpt"] = "5";
            workFlow["flow0_3_FPRW_acpt==asn&!mng==acpt"] = "4";
            workFlow["flow0_3_FPRW_!acpt==asn&!mng==acpt"] = "4";
            workFlow["flow0_3_FPRW_AllCondition"] = "IsAcptEmpty,acpt==crtusr,acpt==asn&mng==acpt,!acpt==asn&mng==acpt,acpt==asn&!mng==acpt,!acpt==asn&!mng==acpt";
            workFlow["flow0_3_HTRW"] = "0";
            workFlow["flow0_3_HTRW_AllCondition"] = "";
            workFlow["flow0_3_ZFP_IsAcptEmpty"] = "3";
            workFlow["flow0_3_ZFP_crtusr==mng"] = "4";
            workFlow["flow0_3_ZFP_!crtusr==mng"] = "3";
            workFlow["flow0_3_ZFP_AllCondition"] = "IsAcptEmpty,crtusr==mng,!crtusr==mng";
            workFlow["flow0_3_GBRW_crtusr==mng"] = "9";
            workFlow["flow0_3_GBRW_crtusr==creater"] = "9";
            workFlow["flow0_3_GBRW_AllCondition"] = "crtusr==mng,crtusr==creater";
            workFlow["flow0_3_CQRW_crtusr==mng"] = "o";
            workFlow["flow0_3_CQRW_crtusr==creater"] = "o";
            workFlow["flow0_3_CQRW_AllCondition"] = "crtusr==mng,crtusr==creater";
            //待分配状态流转
            //待分配状态执行的op
            workFlow["flow0_3_FPRW_IsAcptEmpty_op"] = "UpdAsn";
            workFlow["flow0_3_FPRW_acpt==crtusr_op"] = "UpdState,UpdAsn,UpdAsnor,AddCost,Log";
            workFlow["flow0_3_FPRW_acpt==asn&mng==acpt_op"] = "UpdState,UpdAsnor,Log,AddCost";
            workFlow["flow0_3_FPRW_!acpt==asn&mng==acpt_op"] = "UpdState,UpdAsnor,UpdAsn,Log,AddCost";
            workFlow["flow0_3_FPRW_acpt==asn&!mng==acpt_op"] = "UpdState,UpdAsnor,Log";
            workFlow["flow0_3_FPRW_!acpt==asn&!mng==acpt_op"] = "UpdState,UpdAsnor,UpdAsn,Log";
            workFlow["flow0_3_HTRW_op"] = "UpdState,Log";
            workFlow["flow0_3_ZFP_IsAcptEmpty_op"] = "UpdAsn,Log";
            workFlow["flow0_3_ZFP_crtusr==mng_op"] = "UpdAsn,UpdState,Log";
            workFlow["flow0_3_ZFP_!crtusr==mng_op"] = "UpdAsn,Log";
            workFlow["flow0_3_GBRW_crtusr==mng_op"] = "UpdState,Log,ChildStart";
            workFlow["flow0_3_GBRW_crtusr==creater_op"] = "UpdState,Log,ChildStart";
            workFlow["flow0_3_CQRW_crtusr==mng_op"] = "UpdState,Log";
            workFlow["flow0_3_CQRW_crtusr==creater_op"] = "UpdState,Log";
            //待分配状态执行的op
            //op的参数
            workFlow["flow0_3_FPRW_acpt==crtusr_Log"] = "&crtusr&受理任务单";
            workFlow["flow0_3_FPRW_acpt==asn&mng==acpt_Log"] = "&mng&受理任务单";
            workFlow["flow0_3_FPRW_!acpt==asn&mng==acpt_Log"] = "&mng&受理任务单";
            workFlow["flow0_3_FPRW_acpt==asn&!mng==acpt_Log"] = "&crtusr&分配任务单";
            workFlow["flow0_3_FPRW_!acpt==asn&!mng==acpt_Log"] = "&crtusr&分配任务单";
            workFlow["flow0_3_HTRW_Log"] = "&mng&回退任务单&reason&";
            workFlow["flow0_3_ZFP_IsAcptEmpty_Log"] = "&crtusr&将受理人置空";
            workFlow["flow0_3_ZFP_crtusr==mng_Log"] = "&crtusr&更换受理人为&acptusr";
            workFlow["flow0_3_ZFP_!crtusr==mng_Log"] = "&crtusr&更换受理人为&acptusr";
            workFlow["flow0_3_GBRW_crtusr==mng_Log"] = "&mng&关闭任务单&reason&";
            workFlow["flow0_3_GBRW_crtusr==creater_Log"] = "&mng&关闭任务单&reason&";
            workFlow["flow0_3_CQRW_crtusr==mng_Log"] = "&crtusr&重启任务单";
            workFlow["flow0_3_CQRW_crtusr==creater_Log"] = "&crtusr&重启任务单";
            //op的参数
            ///////////////////待分配状态//////////////////////////////////

            ///////////////////待受理状态////////////////////////////////
            //待受理状态流转
            workFlow["flow0_4_SLRW"] = "5";
            workFlow["flow0_4_SLRW_AllCondition"] = "";
            workFlow["flow0_4_HTRW"] = "3";
            workFlow["flow0_4_HTRW_AllCondition"] = "";
            workFlow["flow0_4_ZFP_!acpt==asn&crtusr==mng"] = "4";
            workFlow["flow0_4_ZFP_!acpt==asn&!crtusr==mng"] = "3";
            workFlow["flow0_4_ZFP_AllCondition"] = "!acpt==asn&crtusr==mng,!acpt==asn&!crtusr==mng";
            workFlow["flow0_4_GBRW_crtusr==mng"] = "9";
            workFlow["flow0_4_GBRW_crtusr==creater"] = "9";
            workFlow["flow0_4_GBRW_AllCondition"] = "crtusr==mng,crtusr==creater";
            workFlow["flow0_4_CQRW_crtusr==mng"] = "o";
            workFlow["flow0_4_CQRW_crtusr==creater"] = "o";
            workFlow["flow0_4_CQRW_AllCondition"] = "crtusr==mng,crtusr==creater";
            //待受理状态流转
            //待受理状态执行的op
            workFlow["flow0_4_SLRW_op"] = "UpdState,Log,AddCost";
            workFlow["flow0_4_HTRW_op"] = "UpdState,Log";
            workFlow["flow0_4_ZFP_!acpt==asn&crtusr==mng_op"] = "UpdAsn,Log";
            workFlow["flow0_4_ZFP_!acpt==asn&!crtusr==mng_op"] = "UpdAsn,UpdState,Log";
            workFlow["flow0_4_GBRW_crtusr==mng_op"] = "UpdState,Log,ChildStart";
            workFlow["flow0_4_GBRW_crtusr==creater_op"] = "UpdState,Log,ChildStart";
            workFlow["flow0_4_CQRW_crtusr==mng_op"] = "UpdState,Log";
            workFlow["flow0_4_CQRW_crtusr==creater_op"] = "UpdState,Log";
            //待受理状态执行的op
            //op的参数
            workFlow["flow0_4_SLRW_Log"] = "&crtusr&受理任务单";
            workFlow["flow0_4_HTRW_Log"] = "&crtusr&回退任务单&reason&";
            workFlow["flow0_4_ZFP_!acpt==asn&crtusr==mng_Log"] = "&crtusr&更换受理人为&acptusr";
            workFlow["flow0_4_ZFP_!acpt==asn&!crtusr==mng_Log"] = "&crtusr&更换受理人为&acptusr";
            workFlow["flow0_4_GBRW_crtusr==mng_Log"] = "&mng&关闭任务单&reason&";
            workFlow["flow0_4_GBRW_crtusr==creater_Log"] = "&mng&关闭任务单&reason&";
            workFlow["flow0_4_CQRW_crtusr==mng_Log"] = "&crtusr&重启任务单";
            workFlow["flow0_4_CQRW_crtusr==creater_Log"] = "&crtusr&重启任务单";
            //op的参数
            ///////////////////待受理状态////////////////////////////////

            ///////////////////////处理中状态//////////////////////////
            //处理中状态流转
            workFlow["flow0_5_WCRW_mng==asn"] = "7";
            workFlow["flow0_5_WCRW_!mng==asn"] = "6";
            workFlow["flow0_5_WCRW_AllCondition"] = "mng==asn,!mng==asn";
            workFlow["flow0_5_ZFP_!acpt==asn&crtusr==mng"] = "5";
            workFlow["flow0_5_ZFP_!acpt==asn&!crtusr==mng"] = "3";
            workFlow["flow0_5_ZFP_AllCondition"] = "!acpt==asn&crtusr==mng,!acpt==asn&!crtusr==mng";
            workFlow["flow0_5_TGSH"] = "Y";
            workFlow["flow0_5_TGSH_AllCondition"] = "";
            workFlow["flow0_5_WTGSH"] = "N";
            workFlow["flow0_5_WTGSH_AllCondition"] = "";
            workFlow["flow0_5_YSTG"] = "C";
            workFlow["flow0_5_YSTG_AllCondition"] = "";
            workFlow["flow0_5_GBRW_crtusr==mng"] = "9";
            workFlow["flow0_5_GBRW_crtusr==creater"] = "9";
            workFlow["flow0_5_GBRW_AllCondition"] = "crtusr==mng,crtusr==creater";
            workFlow["flow0_5_CQRW_crtusr==mng"] = "o";
            workFlow["flow0_5_CQRW_crtusr==creater"] = "o";
            workFlow["flow0_5_CQRW_AllCondition"] = "crtusr==mng,crtusr==creater";
            //处理中状态流转
            //处理中状态执行的op
            workFlow["flow0_5_WCRW_mng==asn_op"] = "UpdState,UpdRate,Log,ChildStart";
            workFlow["flow0_5_WCRW_!mng==asn_op"] = "UpdState,Log";
            workFlow["flow0_5_ZFP_!acpt==asn&crtusr==mng_op"] = "AddCprTask,Log,AddCost";
            workFlow["flow0_5_ZFP_!acpt==asn&!crtusr==mng_op"] = "AddCprTask,UpdState,Log";
            workFlow["flow0_5_TGSH_op"] = "CheckKnlgTask,UpdState,Log,AddCost";
            workFlow["flow0_5_WTGSH_op"] = "CheckKnlgTask,UpdState,Log,AddCost";
            workFlow["flow0_5_YSTG_op"] = "UpdState,Log";
            workFlow["flow0_5_GBRW_crtusr==mng_op"] = "UpdState,Log,ChildStart";
            workFlow["flow0_5_GBRW_crtusr==creater_op"] = "UpdState,Log,ChildStart";
            workFlow["flow0_5_CQRW_crtusr==mng_op"] = "UpdState,Log";
            workFlow["flow0_5_CQRW_crtusr==creater_op"] = "UpdState,Log";
            //处理中状态执行的op
            //op的参数
            workFlow["flow0_5_WCRW_mng==asn_Log"] = "&mng&完成任务单";
            workFlow["flow0_5_WCRW_mng==asn_UpdRate"] = "100";
            workFlow["flow0_5_WCRW_!mng==asn_Log"] = "&crtusr&申请任务单完成确认";
            workFlow["flow0_5_ZFP_!acpt==asn&crtusr==mng_Log"] = "&mng&将任务转给&acptusr";
            workFlow["flow0_5_ZFP_!acpt==asn&!crtusr==mng_Log"] = "&crtusr&申请任务再分配";
            workFlow["flow0_5_TGSH_Log"] = "&crtusr&审核知识通过&reason&";
            workFlow["flow0_5_WTGSH_Log"] = "&crtusr&审核知识未通过&reason&";
            workFlow["flow0_5_TGSH_CheckKnlgTask"] = "oth:&oth&,IsPassed:true,remark:&reason&";
            workFlow["flow0_5_YSTG_Log"] = "&crtusr&确认任务单通过预审";
            workFlow["flow0_5_GBRW_crtusr==mng_Log"] = "&mng&关闭任务单&reason&";
            workFlow["flow0_5_GBRW_crtusr==creater_Log"] = "&mng&关闭任务单&reason&";
            workFlow["flow0_5_CQRW_crtusr==mng_Log"] = "&crtusr&重启任务单";
            workFlow["flow0_5_CQRW_crtusr==creater_Log"] = "&crtusr&重启任务单";
            //op的参数
            //////////////////////处理中状态///////////////////////////


            ///////////////////////预审完成状态//////////////////////////
            //处理中状态流转
            workFlow["flow0_C_WCRW_mng==asn"] = "7";
            workFlow["flow0_C_WCRW_!mng==asn"] = "6";
            workFlow["flow0_C_WCRW_AllCondition"] = "mng==asn,!mng==asn";
            workFlow["flow0_C_GBRW_crtusr==mng"] = "9";
            workFlow["flow0_C_GBRW_crtusr==creater"] = "9";
            workFlow["flow0_C_GBRW_AllCondition"] = "crtusr==mng,crtusr==creater";
            workFlow["flow0_C_CQRW_crtusr==mng"] = "o";
            workFlow["flow0_C_CQRW_crtusr==creater"] = "o";
            workFlow["flow0_C_CQRW_AllCondition"] = "crtusr==mng,crtusr==creater";
            //处理中状态流转
            //处理中状态执行的op
            workFlow["flow0_C_WCRW_mng==asn_op"] = "UpdState,UpdRate,Log,ChildStart";
            workFlow["flow0_C_WCRW_!mng==asn_op"] = "UpdState,Log";
            workFlow["flow0_C_GBRW_crtusr==mng_op"] = "UpdState,Log,ChildStart";
            workFlow["flow0_C_GBRW_crtusr==creater_op"] = "UpdState,Log,ChildStart";
            workFlow["flow0_C_CQRW_crtusr==mng_op"] = "UpdState,Log";
            workFlow["flow0_C_CQRW_crtusr==creater_op"] = "UpdState,Log";
            //处理中状态执行的op
            //op的参数
            workFlow["flow0_C_WCRW_mng==asn_Log"] = "&mng&完成任务单";
            workFlow["flow0_C_WCRW_mng==asn_UpdRate"] = "100";
            workFlow["flow0_C_WCRW_!mng==asn_Log"] = "&crtusr&申请任务单完成确认";
            workFlow["flow0_C_GBRW_crtusr==mng_Log"] = "&mng&关闭任务单&reason&";
            workFlow["flow0_C_GBRW_crtusr==creater_Log"] = "&mng&关闭任务单&reason&";
            workFlow["flow0_C_CQRW_crtusr==mng_Log"] = "&crtusr&重启任务单";
            workFlow["flow0_C_CQRW_crtusr==creater_Log"] = "&crtusr&重启任务单";
            //op的参数
            //////////////////////预审完成状态///////////////////////////



            /////////////////////////////待确认完成状态///////////////////////////
            //待确认完成状态流转
            workFlow["flow0_6_QRWC"] = "7";
            workFlow["flow0_6_QRWC_AllCondition"] = "";
            workFlow["flow0_6_QRWWC"] = "5";
            workFlow["flow0_6_QRWWC_AllCondition"] = "";
            workFlow["flow0_6_GBRW_crtusr==mng"] = "9";
            workFlow["flow0_6_GBRW_crtusr==creater"] = "9";
            workFlow["flow0_6_GBRW_AllCondition"] = "crtusr==mng,crtusr==creater";
            workFlow["flow0_6_CQRW_crtusr==mng"] = "o";
            workFlow["flow0_6_CQRW_crtusr==creater"] = "o";
            workFlow["flow0_6_CQRW_AllCondition"] = "crtusr==mng,crtusr==creater";
            //待确认完成状态流转
            //待确认完成执行的op
            workFlow["flow0_6_QRWC_op"] = "UpdState,UpdRate,Log,ChildStart";
            workFlow["flow0_6_QRWWC_op"] = "UpdState,Log";
            workFlow["flow0_6_GBRW_crtusr==mng_op"] = "UpdState,Log,ChildStart";
            workFlow["flow0_6_GBRW_crtusr==creater_op"] = "UpdState,Log,ChildStart";
            workFlow["flow0_6_CQRW_crtusr==mng_op"] = "UpdState,Log";
            workFlow["flow0_6_CQRW_crtusr==creater_op"] = "UpdState,Log";
            //待确认完成执行的op
            //op的参数
            workFlow["flow0_6_QRWC_Log"] = "&mng&确认完成任务单";
            workFlow["flow0_6_QRWC_UpdRate"] = "100";
            workFlow["flow0_6_QRWWC_Log"] = "&mng&确认任务单未完成&reason&";
            workFlow["flow0_6_GBRW_crtusr==mng_Log"] = "&mng&关闭任务单&reason&";
            workFlow["flow0_6_GBRW_crtusr==creater_Log"] = "&mng&关闭任务单&reason&";
            workFlow["flow0_6_CQRW_crtusr==mng_Log"] = "&crtusr&重启任务单";
            workFlow["flow0_6_CQRW_crtusr==creater_Log"] = "&crtusr&重启任务单";
            //op的参数
            /////////////////////////////待确认完成状态///////////////////////////

            //////////////////////////////完成状态/////////////////////////////////
            //完成状态流转
            workFlow["flow0_7_JXWC_crtusr==mng"] = "5";
            workFlow["flow0_7_JXWC_crtusr==creater"] = "5";
            workFlow["flow0_7_JXWC_AllCondition"] = "crtusr==mng,crtusr==creater";
            workFlow["flow0_7_CJRW"] = "6";
            //完成状态流转
            //完成状态执行的op
            workFlow["flow0_7_JXWC_crtusr==mng_op"] = "UpdState,Log";
            workFlow["flow0_7_JXWC_crtusr==creater_op"] = "UpdState,Log";
            workFlow["flow0_7_CJRW_op"] = "UpdState,Log";
            //完成状态执行的op
            //op的参数
            workFlow["flow0_7_JXWC_crtusr==mng_Log"] = "&crtusr&将此任务重启继续完成";
            workFlow["flow0_7_JXWC_crtusr==creater_Log"] = "&crtusr&将此任务重启继续完成";
            workFlow["flow0_7_CJRW_Log"] = "因上级任务重启而重启";
            //op的参数
            //////////////////////////////完成状态/////////////////////////////////
            //////////////////////////////////////////流程0（丁利流程）//////////////////////////////////////

            //////////////////////////////////////////流程1（龚毅流程）//////////////////////////////////////
            ///////////////创建状态/////////////////////////
            //创建状态流转
            workFlow["flow1_0_CJRW_IsAsnEmpty"] = "3";
            workFlow["flow1_0_CJRW_!IsAsnEmpty"] = "5";
            workFlow["flow1_0_CJRW_AllCondition"] = "IsAsnEmpty,!IsAsnEmpty";
            workFlow["flow1_0_ZFP_!acpt==asn"] = "0";
            workFlow["flow1_0_ZFP_AllCondition"] = "!acpt==asn";
            workFlow["flow1_0_WCRW"] = "3";
            workFlow["flow1_0_WCRW_AllCondition"] = "";
            workFlow["flow1_0_GBRW"] = "9";
            workFlow["flow1_0_GBRW_AllCondition"] = "";
            workFlow["flow1_0_CQRW"] = "o";
            workFlow["flow1_0_CQRW_AllCondition"] = "";
            //创建状态流转
            //创建执行的op
            workFlow["flow1_0_CJRW_IsAsnEmpty_op"] = "UpdState";
            workFlow["flow1_0_CJRW_!IsAsnEmpty_op"] = "UpdState,,UpdAsnor,AddCost";
            workFlow["flow1_0_ZFP_!acpt==asn_op"] = "UpdAsn";
            workFlow["flow1_0_WCRW_op"] = "UpdState";
            workFlow["flow1_0_GBRW_op"] = "UpdState,Log,ChildStart";
            workFlow["flow1_0_CQRW_op"] = "UpdState,Log";
            //op的参数
            workFlow["flow1_0_GBRW_Log"] = "&crtusr&关闭任务单&reason&";
            workFlow["flow1_0_CQRW_Log"] = "&crtusr&重启任务单";


            ////////////////创建状态//////////////////////

            //////////////////////待分配状态////////////////////////////////
            //待分配状态流转
            workFlow["flow1_3_FPRW"] = "5";
            workFlow["flow1_3_FPRW_AllCondition"] = "";
            workFlow["flow1_3_HTRW"] = "0";
            workFlow["flow1_3_HTRW_AllCondition"] = "";
            workFlow["flow1_3_ZFP"] = "5";
            workFlow["flow1_3_ZFP_AllCondition"] = "";
            workFlow["flow1_3_GBRW"] = "9";
            workFlow["flow1_3_GBRW_AllCondition"] = "";
            workFlow["flow1_3_CQRW"] = "o";
            workFlow["flow1_3_CQRW_AllCondition"] = "";
            //待分配状态流转
            //待分配状态执行的op
            workFlow["flow1_3_FPRW_op"] = "UpdState,UpdAsnor,Log,AddCost";
            workFlow["flow1_3_HTRW_op"] = "UpdState,Log";
            workFlow["flow1_3_ZFP_op"] = "UpdAsn,UpdAsnor,AddCost,UpdState,Log";
            workFlow["flow1_3_GBRW_op"] = "UpdState,Log,ChildStart";
            workFlow["flow1_3_CQRW_op"] = "UpdState,Log";
            //待分配状态执行的op
            //op的参数
            workFlow["flow1_3_FPRW_Log"] = "&mng&分配任务单";
            workFlow["flow1_3_HTRW_Log"] = "&mng&回退任务单&reason&";
            workFlow["flow1_3_ZFP_Log"] = "&crtusr&更换受理人为&acptusr";
            workFlow["flow1_3_GBRW_Log"] = "&crtusr&关闭任务单&reason&";
            workFlow["flow1_3_CQRW_Log"] = "&crtusr&重启任务单";
            //op的参数
            ///////////////////待分配状态//////////////////////////////////

            ///////////////////待受理状态////////////////////////////////
            //待受理状态流转
            //待受理状态流转
            //待受理状态执行的op
            //待受理状态执行的op
            //op的参数
            //op的参数
            ///////////////////待受理状态////////////////////////////////

            ///////////////////////处理中状态//////////////////////////
            //处理中状态流转
            workFlow["flow1_5_WCRW"] = "7";
            workFlow["flow1_5_WCRW_AllCondition"] = "";
            workFlow["flow1_5_ZFP"] = "5";
            workFlow["flow1_5_ZFP_AllCondition"] = "";
            workFlow["flow1_5_TGSH"] = "Y";
            workFlow["flow1_5_TGSH_AllCondition"] = "";
            workFlow["flow1_5_WTGSH"] = "N";
            workFlow["flow1_5_WTGSH_AllCondition"] = "";
            workFlow["flow1_5_GBRW"] = "9";
            workFlow["flow1_5_GBRW_AllCondition"] = "";
            workFlow["flow1_5_CQRW"] = "o";
            workFlow["flow1_5_CQRW_AllCondition"] = "";
            //处理中状态流转
            //处理中状态执行的op
            workFlow["flow1_5_WCRW_op"] = "UpdState,UpdRate,Log,ChildStart";
            workFlow["flow1_5_ZFP_op"] = "AddCprTask,Log,AddCost";
            workFlow["flow1_5_TGSH_op"] = "CheckKnlgTask,UpdState,Log,AddCost";
            workFlow["flow1_5_WTGSH_op"] = "CheckKnlgTask,UpdState,Log,AddCost";
            workFlow["flow1_5_GBRW_op"] = "UpdState,Log,ChildStart";
            workFlow["flow1_5_CQRW_op"] = "UpdState,Log";
            //处理中状态执行的op
            //op的参数
            workFlow["flow1_5_WCRW_Log"] = "&crtusr&完成任务单";
            workFlow["flow1_5_WCRW_UpdRate"] = "100";
            workFlow["flow1_5_ZFP_Log"] = "&crtusr&将任务转给&acptusr";
            workFlow["flow1_5_TGSH_Log"] = "&crtusr&审核知识通过&reason&";
            workFlow["flow1_5_WTGSH_Log"] = "&crtusr&审核知识未通过&reason&";
            workFlow["flow1_5_TGSH_CheckKnlgTask"] = "oth:&oth&,IsPassed:true,remark:&reason&";
            workFlow["flow1_5_WTGSH_CheckKnlgTask"] = "oth:&oth&,IsPassed:false,remark:&reason&";
            workFlow["flow1_5_GBRW_Log"] = "&crtusr&关闭任务单&reason&";
            workFlow["flow1_5_CQRW_Log"] = "&crtusr&重启任务单";
            //op的参数
            //////////////////////处理中状态///////////////////////////

            /////////////////////////////待确认完成状态///////////////////////////
            //待确认完成状态流转
            //待确认完成状态流转
            //待确认完成执行的op
            //待确认完成执行的op
            //op的参数
            //op的参数
            /////////////////////////////待确认完成状态///////////////////////////

            //////////////////////////////完成状态/////////////////////////////////
            //完成状态流转
            workFlow["flow1_7_JXWC"] = "5";
            workFlow["flow1_7_JXWC_AllCondition"] = "";
            workFlow["flow1_7_CJRW"] = "5";
            //完成状态流转
            //完成状态执行的op
            workFlow["flow1_7_JXWC_op"] = "UpdState,Log";
            workFlow["flow1_7_CJRW_op"] = "UpdState,Log";
            //完成状态执行的op
            //op的参数
            workFlow["flow1_7_JXWC_Log"] = "&crtusr&将此任务重启继续完成";
            workFlow["flow1_7_CJRW_Log"] = "因上级任务重启而重启";
            //op的参数
            //////////////////////////////完成状态/////////////////////////////////

            //////////////////////////////////////////流程1（龚毅流程）//////////////////////////////////////


            ////////////////wq任务单流程图相关////////////////////////////////





            //////////////////////错误码///////////////////////////////
            errnohs = new Hashtable();
            /////////////全局///////////////////////
            errnohs["X001"] = "系统错误！";
            errnohs["X006"] = "文件格式不符，请检查文件格式！";
            errnohs["X009"] = "获得页面参数失败！";
            errnohs["X007"] = "内部错误,[文件未能上传到文件服务器]";
            errnohs["X008"] = "内部错误,[文件转移到应用服务器失败]";
            /////////////全局///////////////////////


            //wq 人员权限相关 //////////////////////
            errnohs["XAAU"] = "更新人员表失败 ！";
            errnohs["XAAI"] = "添加人员表失败 ！";
            errnohs["XAA1"] = "人员ID未找到！";
            errnohs["XAA2"] = "人员ID重复！";
            errnohs["XA04"] = "您没有用户管理权限或添加修改此条数据的权限！";
            errnohs["XAAS"] = "访问人员表失败！";
            errnohs["XA11"] = "人员所属部门不能为空！";
            errnohs["XA12"] = "人员所属部门填写不符合规则！";
            errnohs["XA13"] = "用户ID不能为空 ！";
            errnohs["XA14"] = "被代理人不能为空 ！";
            errnohs["XA15"] = "代理人不能为空 ！";
            errnohs["XA16"] = "请用智能提示选出系统存在的人员 ！";
            errnohs["XA17"] = "代理人与被代理人不能为同一人 ！";
            errnohs["XA19"] = "用户动态权限未设置！";
            errnohs["XAA4"] = "用户已删除！";
            errnohs["XAA8"] = "用户角色未设置！";
            errnohs["XABS"] = "访问机构表失败！";
            errnohs["XAB1"] = "非用户管理范围，请重新选择！";
            errnohs["XC01"] = "此角色不存在！";
            errnohs["XC02"] = "不能设置比自己权限高的角色！";
            errnohs["XC03"] = "不能设置自己没有的权限！";
            errnohs["XC04"] = "权限点不可以被代理！";
            errnohs["XC05"] = "用户状态处于删除状态，不能设置权限，请重置用户状态后再设置！";
            errnohs["XC06"] = "用户状态处于离职状态，不能设置权限，请重置用户状态后再设置！";
            errnohs["XC07"] = "用户状态处于未激活状态，不能设置权限，请重置用户状态后再设置！";
            errnohs["XAF2"] = "动态授权，设置的日期已过期！";
            errnohs["XAF3"] = "该代理人已在代理您的权限，请点击编辑！";
            errnohs["XAF4"] = "插入动态授权表，动态授权流水表失败！";
            errnohs["XAF5"] = "您无权删除他人的授权信息！";
            errnohs["XAF6"] = "被代理人非管理范围！";
            errnohs["XAF7"] = "代理人非管理范围！";
            errnohs["XAF8"] = "代理权限为空，请至少选择一个权限点！";
            errnohs["XAFI"] = "向动态授权表中插入数据时发生错误！";
            errnohs["XAFU"] = "向动态授权表中更新数据时发生错误！";
            errnohs["XAFD"] = "删除动态授权信息失败！";
            errnohs["XAEI"] = "向动态授权流水表中插入数据时发生错误！";
            errnohs["XAD0"] = "批量更新或插入信息表失败！";
            errnohs["XAD1"] = "更新或插入信息表失败！";
            errnohs["XAD2"] = "人员状态不正常，不能作为被替换人员！";

            //wq 人员权限相关 //////////////////////
            //wq 应用系统相关 //////////////////////
            errnohs["XWAI"] = "添加应用系统失败！";
            errnohs["XWAU"] = "更新应用系统失败！";
            errnohs["XWAD"] = "删除应用系统失败！";
            errnohs["XWA7"] = "获得系统列表失败！";
            errnohs["XWBU"] = "更新运维人员信息失败！";
            errnohs["XWA0"] = "系统ID不能为空！";
            errnohs["XWA1"] = "父系统不能为空！";
            //wq 应用系统相关 //////////////////////

            //wq 值班日志相关 //////////////////////
            errnohs["XWG0"] = "日期格式不正确！";

            //wq 值班表相关 

            //wq 值班表相关 //////////////////////
            errnohs["XAG0"] = "获得页面参数失败！";
            errnohs["XAG1"] = "未找到可参与值班的人员！";
            errnohs["XAG2"] = "机构下人员重名！";
            errnohs["XAG3"] = "日期未找到！";
            //wq 值班表相关 //////////////////////


            //wq 任务单相关

            errnohs["XW01"] = "获得页面参数失败!";
            errnohs["XW01"] = "任务单ID不能为空!";
            errnohs["XW02"] = "任务经理不能为空!";
            errnohs["XW03"] = "任务名称不能为空!";
            errnohs["XW04"] = "任务所属机构不能为空!";
            errnohs["XW05"] = "未找到匹配路径!";
            errnohs["XW06"] = "更新任务单失败!";
            errnohs["XW07"] = "更新完成度缺少参数!";
            errnohs["XW08"] = "添加任务单日志缺少参数!";
            errnohs["XW09"] = "上级任务未完成，不能执行此动作!";
            errnohs["XW10"] = "计算可见范围，缺少受理部门参数!";
            errnohs["XW11"] = "计算可见范围，缺少范围参数!";
            errnohs["XW12"] = "查找该任务信息失败!";
            errnohs["XW13"] = "查找该任务信息失败!";
            errnohs["XW14"] = "执行动作相应的操作失败!";
            errnohs["XW15"] = "添加系统日志失败!";
            errnohs["XW16"] = "添加工作量失败!";
            errnohs["XW17"] = "更新任务状态失败!";
            errnohs["XW18"] = "更新完成度失败!";
            errnohs["XW19"] = "更新受理人失败!";
            errnohs["XW20"] = "更新分配人失败!";
            errnohs["XW21"] = "单条插入数据库失败!";
            errnohs["XW22"] = "更新分解表任务是否为最新失败!";
            errnohs["XW23"] = "更新变更责任人失败!";
            //wq 任务单相关

            /////////////////////错误码//////////////////////////////


            ///////////////////成功提示信息/////////////////////////////
            scuhs = new Hashtable();
            //wq 人员权限相关 ////////////////////////////////////////
            scuhs["SA00"] = "更新人员权限成功";
            scuhs["SA01"] = "更新人员成功，已按权限码更新为匹配的角色";
            scuhs["SA02"] = "更新人员成功，已设置静态权限调整";
            scuhs["SA03"] = "添加动态权限成功";
            scuhs["SA04"] = "删除动态权限成功";
            scuhs["SA05"] = "更新动态权限成功";
            scuhs["SA06"] = "重置人员状态成功";
            scuhs["SA07"] = "重置人员密码成功";
            scuhs["SA08"] = "设置人员排班状态成功";
            scuhs["SAAI"] = "添加人员成功";
            scuhs["SAAU"] = "更新人员基本信息成功";
            scuhs["SABU"] = "更新机构基本信息成功";
            scuhs["SAAD"] = "删除人员基本信息成功";
            scuhs["SWBU"] = "更新应用系统维护信息成功";
            scuhs["SWAI"] = "新增系统信息成功";
            scuhs["SWAU"] = "更新系统信息成功";
            scuhs["SWAD"] = "删除系统信息成功";
            //wq 人员权限相关 //////////////////////////////////////////

            //wq 值班日志相关 ////////////////////////////////////////
            scuhs["SWGU"] = "更新值班日志成功";
            scuhs["SWAH"] = "更新值班表成功";
            //wq 人员权限相关 //////////////////////////////////////////

            scuhs["SDEI"] = "新建任务单成功！";
            scuhs["SDFU"] = "更新任务单成功！";
            scuhs["SDGI"] = "更新日志成功！";
            scuhs["SDJU"] = "更新问题跟踪成功！";
            scuhs["SDKU"] = "更新变更预审成功！";
            scuhs["SCGI"] = "添加关注的任务单成功！";
            scuhs["SCGD"] = "取消关注的任务单成功！";

            scuhs["SADU"] = "重新选择继任者成功！";
            scuhs["SAD0"] = "忽略提示信息成功！";
            
            ///////////////////成功提示信息//////////////////////////////

            ////////////////wq////////////////////////////////////////
            //权限点名称
            authcnamhs = new Hashtable();
            authcnamhs["TskMng"] = "任务管理";
            authcnamhs["KnlgMng"] = "知识库管理";
            authcnamhs["AdvInfoSel"] = "高级信息查询";
            authcnamhs["SysInfoMT"] = "应用维护";
            authcnamhs["DataLeadIn"] = "数据录入";
            authcnamhs["RptMng"] = "报表管理";
            authcnamhs["DocMng"] = "文档管理";
            authcnamhs["NoteMng"] = "公告管理";
            authcnamhs["SendMS"] = "短信息服务";
            authcnamhs["UsrMng"] = "人员管理";
            authcnamhs["OrgMng"] = "部门信息维护";
            authcnamhs["AMPCfg"] = "AMP配置";


            //权限点哈希码
            authhs = new Hashtable();
            authhs["TskMng"] = "00";
            authhs["KnlgMng"] = "01";
            authhs["AdvInfoSel"] = "10";
            authhs["SysInfoMT"] = "11";
            authhs["DataLeadIn"] = "20";
            authhs["RptMng"] = "30";
            authhs["DocMng"] = "31";
            authhs["NoteMng"] = "32";
            authhs["SendMS"] = "40";
            authhs["UsrMng"] = "50";
            authhs["OrgMng"] = "51";
            authhs["AMPCfg"] = "52";


            //数据限定哈希码
            datalimths = new Hashtable();
            datalimths["TskMng"] = "S1";
            datalimths["KnlgMng"] = "S1";
            datalimths["AdvInfoSel"] = "J1";
            datalimths["SysInfoMT"] = "U1";
            datalimths["DataLeadIn"] = "S1";
            datalimths["RptMng"] = "S1";
            datalimths["DocMng"] = "S1";
            datalimths["NoteMng"] = "S1";
            datalimths["SendMS"] = "S1";
            datalimths["UsrMng"] = "J1";
            datalimths["OrgMng"] = "S0";
            datalimths["AMPCfg"] = "S0";

            //角色名称
            rolnamhs = new NoSortHashtable();            
            rolnamhs["T1"] = "员工一级";
            rolnamhs["T2"] = "员工二级";
            rolnamhs["T3"] = "员工三级";
            rolnamhs["T4"] = "员工四级";
            rolnamhs["M1"] = "维护员一级";
            rolnamhs["M2"] = "维护员二级";
            rolnamhs["M3"] = "维护员三级";
            rolnamhs["M4"] = "维护员四级";
            rolnamhs["L1"] = "组长";
            rolnamhs["L2"] = "处长";
            rolnamhs["L3"] = "总经理";
            rolnamhs["L4"] = "行长";
            rolnamhs["A1"] = "系统管理员";
            
            //角色级别
            rollevhs = new Hashtable();
            rollevhs["T"] = "1";
            rollevhs["M"] = "2";
            rollevhs["L"] = "3";
            rollevhs["A"] = "4";


            //职位
            titlehs = new NoSortHashtable();
            titlehs["0"] = "员工";
            titlehs["0.9"] = "副组长";
            titlehs["1"] = "组长";
            titlehs["2.91"] = "专家";
            titlehs["1.9"] = "高级专家";
            titlehs["1.8"] = "专家";
            titlehs["2"] = "副处长";
            titlehs["2.9"] = "副处长";
            titlehs["2.8"] = "副处长";
            titlehs["2.7"] = "副处长";
            titlehs["2.6"] = "副处长";
            titlehs["2.5"] = "副处长";
            titlehs["2.1"] = "负责人";
            titlehs["2.2"] = "资深专员";
            titlehs["3"] = "处长";
            titlehs["3.9"] = "处长";
            titlehs["3.8"] = "处长";
            titlehs["3.7"] = "处长";
            titlehs["4"] = "副总经理";
            titlehs["4.99"] = "资深独立审批人";
            titlehs["4.98"] = "资深独立审批人";
            titlehs["4.91"] = "总工程师";
            titlehs["4.9"] = "副总经理";
            titlehs["4.8"] = "副总经理";
            titlehs["4.7"] = "副总经理";
            titlehs["4.6"] = "副总经理";
            titlehs["4.5"] = "副总经理";
            titlehs["4.1"] = "总经理助理";
            titlehs["4.2"] = "副总工程师";
            titlehs["4.3"] = "总工程师";
            titlehs["4.4"] = "学会副秘书长";
            titlehs["4.49"] = "总编";
            titlehs["4.48"] = "副总编";
            titlehs["5"] = "总经理";
            titlehs["5.9"] = "总经理";
            titlehs["5.2"] = "社长";
            titlehs["5.1"] = "总工程师";
            titlehs["6"] = "副书记";
            titlehs["7"] = "书记";
            titlehs["7.3"] = "总审计师局长";
            titlehs["7.2"] = "正局级";
            titlehs["7.1"] = "副局长";
            titlehs["7.5"] = "局长";
            titlehs["6.5"] = "董秘兼董办主任";
            titlehs["6.3"] = "副秘书长";
            titlehs["6.4"] = "副主任";
            titlehs["6.41"] = "副主任";
            titlehs["6.6"] = "主任";
            titlehs["3.99"] = "主任助理";
            titlehs["8"] = "副行长";
            titlehs["9"] = "行长";
            titlehs["9.9"] = "行长";
            titlehs["9.8"] = "行长";
            titlehs["9.7"] = "行长";
            titlehs["9.6"] = "行长";
            titlehs["9.5"] = "行长";
            titlehs["9.4"] = "行长";
            titlehs["9.3"] = "行长";
            titlehs["9.2"] = "行长";
            titlehs["9.1"] = "行长";
            titlehs["9.99"] = "董事长";

            activehs = new Hashtable();

            activehs["0"] = "正常";
            activehs["1"] = "删除";
            activehs["2"] = "休假";
            activehs["3"] = "离职";
            activehs["4"] = "未激活";
            activehs["5"] = "出差";


            //人员是否在线
            statushs = new Hashtable();
            statushs["0"] = "是";
            statushs["1"] = "否";

            //运维角色
            mtnumhs =new Hashtable();
            mtnumhs["1"] = "一线A角";
            mtnumhs["2"] = "一线B角";
            mtnumhs["3"] = "二线A角";
            mtnumhs["4"] = "二线B角";
            mtnumhs["5"] = "三线A角";
            mtnumhs["6"] = "三线B角";


            //应用系统类型
            systype = new Hashtable();
            systype["0"] = "生产类系统";
            systype["1"] = "平台类系统";
            systype["2"] = "业务管理类系统";
            systype["3"] = "运行支持与安全管理类系统";

            //文档类型
            doctype = new Hashtable();
            doctype["01"] ="需求";
            doctype["02"]="设计";
            doctype["03"]="实现";
            doctype["04"]="测试";
            doctype["05"]="发布";
            doctype["1"]="知识库";
            doctype["2"]="报表";
            doctype["3"]="公告";
            doctype["4"]="规章制度";
            doctype["5"]="通讯录";
            doctype["9"] = "其它";

            //可见范围
            scopehs = new Hashtable();
            scopehs["0"] = "本人可见";
            scopehs["1"] = "本组可见";
            scopehs["2"] = "本处可见";
            scopehs["3"] = "本部可见";
            scopehs["9"] = "所有人可见";

            //////////////////////////////end wq //////////////////////////////////////
        }

        /// <summary>
        /// 根据码值返回错误信息
        /// </summary>
        /// <param name="msgCod"></param>
        /// <returns></returns>
        public static string GetErrMsg(string errCod)
        {
            string res = "";
            if (errnohs[errCod] != null)
            {
                res = errnohs[errCod].ToString();
            }
            else
            {
                res = errCod;
            }
            return res;
        }

        /// <summary>
        /// 根据码值返回错误码+错误信息[X001]获得页面参数错误
        /// </summary>
        /// <param name="errCod"></param>
        /// <returns></returns>
        public static string GetErrCodMsg(string errCod)
        {
            string res = "";
            if (errnohs[errCod] != null)
            {
                res = "[" + errCod + "]" + errnohs[errCod].ToString();
            }
            else
            {
                res = errCod;
            }
            return res;
        }

        /// <summary>
        /// 根据码值返回成功信息
        /// </summary>
        /// <param name="msgCod"></param>
        /// <returns></returns>
        public static string GetSucMsg(string msgCod)
        {
            string res = "";
            if (scuhs[msgCod] != null)
            {
                res = scuhs[msgCod].ToString();
            }
            return res;
        }

        /// <summary>
        /// 返回权限的下标码值
        /// </summary>
        /// <param name="authstr"></param>
        /// <returns></returns>
        public static string GetAuthCod(string authstr)
        {
            if (authhs[authstr] == null)
            {
                return "0";
            }
            return authhs[authstr].ToString();
        }

        /// <summary>
        /// 根据权限下标值，返回权限简称
        /// </summary>
        /// <param name="autcod"></param>
        /// <returns></returns>
        public static string GetAuthNam(string authcod)
        {
            foreach (DictionaryEntry de in authhs)
            {
                if (string.Equals(de.Value.ToString(), authcod))
                {
                    return de.Key.ToString();
                }

            }
            return "";

        }

        /// <summary>
        /// 根据权限下标值，返回权限中文名称
        /// </summary>
        /// <param name="autcod"></param>
        /// <returns></returns>
        public static string GetAuthCNam(string authcod)
        {
            string authnam = GetAuthNam(authcod);
            if (string.IsNullOrEmpty(authnam))
            {
                return "";
            }
            return authcnamhs[authnam].ToString();

        }


        /// <summary>
        /// 返回权限的数据范围限定
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string GetAuthDataOrgLmt(string authstr)
        {
            return datalimths[authstr].ToString().Substring(0, 1);
        }

        /// <summary>
        /// 返回可否动态授权
        /// </summary>
        /// <param name="authstr"></param>
        /// <returns></returns>
        public static string GetDAuthFlg(string authstr)
        {
            return datalimths[authstr].ToString().Substring(1, 1);
        }

        /// <summary>
        ///  返回角色的级别
        /// </summary>
        /// <param name="rolnam"></param>
        /// <returns></returns>
        public static string GetRolLev(string rolnam)
        {
            if (string.IsNullOrEmpty(rolnam))
            {
                return "0";
            }
            return rollevhs[rolnam].ToString();
        }


        /// <summary>
        /// 返回所有角色的数组
        /// </summary>
        /// <returns></returns>
        public static string[] GetRolList()
        {
            string[] res = new string[rolnamhs.Count];
            int i = 0;
            foreach (string str in rolnamhs.Keys)
            {
                res[i] = str;
                i++;
            }
            return res;
        }

        /// <summary>
        /// 取得系统最小时间
        /// </summary>
        /// <returns></returns>
        public static string GetMinDate()
        {
            return glbhs["mindate"].ToString();
        }


        /// <summary>
        /// 取得系统最大时间
        /// </summary>
        /// <returns></returns>
        public static string GetMaxDate()
        {
            return glbhs["maxdate"].ToString();
        }


        /// <summary>
        /// 取得职别名称
        /// </summary>
        /// <param name="titleID"></param>
        /// <returns></returns>
        public static string GetUsrTitleNam(string titleID)
        {
            if (titleID == "4.9" || titleID == "4.8" || titleID == "4.7" || titleID == "4.6" || titleID == "4.5")
            {
                return titlehs["4"].ToString();
            }

            if (titlehs.Contains(titleID))
            {
                return titlehs[titleID].ToString();
            }
            return titlehs["0"].ToString();
        }

        /// <summary>
        /// 取得职别ID
        /// </summary>
        /// <param name="titleNam"></param>
        /// <returns></returns>
        public static string GetUsrTitleID(string titleNam)
        {
            if (string.IsNullOrEmpty(titleNam))
            {
                return "0";
            }
            foreach (DictionaryEntry de in titlehs)
            {
                if (string.Equals(de.Value.ToString(), titleNam))
                {
                    return de.Key.ToString();
                }

            }
            return "-1";
        }

        /// <summary>
        /// 判断是否含有某职别名称
        /// </summary>
        /// <param name="titleNam"></param>
        /// <returns></returns>
        public static bool IsHasUsrTitleVal(string titleNam)
        {
            
            if (string.IsNullOrEmpty(titleNam))
            {
                return false;
            }
            if (titlehs.ContainsValue(titleNam))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 取得职位名称列表
        /// </summary>
        /// <param name="titleNam"></param>
        /// <returns></returns>
        public static NoSortHashtable GetTitleNamList()
        {
            return titlehs;
        }

        /// <summary>
        /// 取得人员状态名称
        /// </summary>
        /// <param name="activeID"></param>
        /// <returns></returns>
        public static string GetUsrActiveNam(string activeID)
        {

            if (activehs.Contains(activeID))
            {
                return activehs[activeID].ToString();
            }
            return activehs["0"].ToString();
        }

        /// <summary>
        /// 取得角色的名称列表
        /// </summary>
        /// <param name="titleNam"></param>
        /// <returns></returns>
        public static NoSortHashtable GetRolNamList()
        {
            return rolnamhs;
        }

        /// <summary>
        /// 通过角色简称获得角色中文名称
        /// </summary>
        /// <param name="rolnam"></param>
        /// <returns></returns>
        public static string GetRolCNam(string rolnam)
        {
            if (rolnamhs.Contains(rolnam))
            {
                return rolnamhs[rolnam].ToString();
            }
            else
            {
                return rolnamhs["T1"].ToString();
            }
        }


        /// <summary>
        /// 通过人员在线与否状态显示信息
        /// </summary>
        /// <returns></returns>
        public static string GetUsrStatus(string status)
        {

            if (statushs.Contains(status))
            {
                return statushs[status].ToString();
            }
            else
            {
                return statushs["1"].ToString();
            }
        }

        /// <summary>
        /// 通过运维角色码获得角色名称
        /// </summary>
        /// <param name="mtnum"></param>
        /// <returns></returns>
        public static string GetMtRol(string mtnum)
        {
            if (mtnumhs.Contains(mtnum))
            {
                return mtnumhs[mtnum].ToString();
            }
            else
            {
                return mtnumhs["1"].ToString();
            }
        }

        /// <summary>
        /// 通过运维名称获得角色码
        /// </summary>
        /// <param name="mtrol"></param>
        /// <returns></returns>
        public static string GetMtNum(string mtrol)
        {
            foreach (DictionaryEntry de in mtnumhs)
            {
                if (string.Equals(de.Value.ToString(), mtrol))
                {
                    return de.Key.ToString();
                }

            }
            return "1";
        }

        /// <summary>
        /// 根据类型码，返回应用系统类别名称
        /// </summary>
        /// <param name="typeNum"></param>
        /// <returns></returns>
        public static string GetSysType(string typeNum)
        {
            if (systype.Contains(typeNum))
            {
                return systype[typeNum].ToString();
            }
            else
            {
                return typeNum;
            }

        }

        /// <summary>
        /// 返回应用系统类型码
        /// </summary>
        /// <param name="strSystype"></param>
        /// <returns></returns>
        public static string GetSysTypeNum(string strSystype)
        {
            foreach (DictionaryEntry de in systype)
            {
                if (string.Equals(de.Value.ToString(), strSystype))
                {
                    return de.Key.ToString();
                }

            }
            return "0";

        }
        /// <summary>
        /// 通过0,或1返回是或否
        /// </summary>
        /// <param name="str1Or0"></param>
        /// <returns></returns>
        public static string GetYesNo(string str1Or0)
        {
            if(string.IsNullOrEmpty(str1Or0))
            {
                return "否";
            }
            if (string.Equals("0", str1Or0.Trim()))
            {
                return "是";
            }
            else if (string.Equals("1", str1Or0.Trim()))
            {
                return "否";
            }
            else
            {
                return "否";
            }
        }
        public static string GetYesNoNum(string strYesOrNo)
        {
            if (string.IsNullOrEmpty(strYesOrNo))
            {
                return "1";
            }

            else if (string.Equals("是", strYesOrNo.Trim()))
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }

        /// <summary>
        /// 通过文档类型码返回文档类型名称
        /// </summary>
        /// <param name="doctype"></param>
        /// <returns></returns>
        public static string GetDocTypeNam(string doctypenum)
        {
            if (doctype.Contains(doctypenum))
            {
                return doctype[doctypenum].ToString();
            }
            else
            {
                return doctype["9"].ToString();
            }
        }



        /// <summary>
        /// 返回成功信息
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string GetTaskSucMsg(string op)
        {
            string res = string.Empty;
            if (workFlow.ContainsKey(op))
            {
                res = workFlow[op].ToString();
            }
            else
            {
                res = op;
            }
            res = res + "成功";
            return res;
        }

        /// <summary>
        /// 获得查询范围中文名称
        /// </summary>
        /// <param name="scopeid"></param>
        /// <returns></returns>
        public static string GetScopeNam(string scopeid)
        {
            if (scopehs.Contains(scopeid))
            {
                return scopehs[scopeid].ToString();
            }
            else
            {
                return scopehs["9"].ToString();
            }
        }

        /// <summary>
        /// 根据可见范围中文返回对应id
        /// </summary>
        /// <param name="scopenam"></param>
        /// <returns></returns>
        public static string GetScope(string scopenam)
        {

            foreach (DictionaryEntry de in scopehs)
            {
                if (string.Equals(de.Value.ToString(), scopenam))
                {
                    return de.Key.ToString();
                }

            }
            return "9";
        }

        /// <summary>
        /// 根据section ,key读取 value
        /// </summary>
        /// <param name="scopenam"></param>
        /// <returns></returns>
        public static string Get(string section, string key)
        {

            try
            {
                //获得当前程序存放目录
                string strRoot = AppDomain.CurrentDomain.BaseDirectory;

                string strFullName = strRoot + @"INI\DBtool.ini";

                if (!File.Exists(strFullName))
                {
                    return "-1";
                }

                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(strFullName);

                XmlNode xnNode;
                XmlAttributeCollection xmlAC;
                XmlAttribute xmlAttKey;
                XmlAttribute xmlAttValue;
                string strKey = string.Empty;
                string strValue;
                XmlNodeList xmlNL;
                IEnumerator iEnum;
                Hashtable xmlhs = new Hashtable();

                XmlNode xmlAMP = xmlDoc.SelectSingleNode("AMP");
                if (null == xmlAMP)
                {
                    return "-1";
                }

                XmlNode xmlSection = xmlAMP.SelectSingleNode(section);
                if (null == xmlSection)
                {
                    return "-1";
                }

                xmlNL = xmlSection.ChildNodes;
                iEnum = xmlNL.GetEnumerator();

                while (iEnum.MoveNext())
                {
                    xnNode = (XmlNode)iEnum.Current;
                    xmlAC = xnNode.Attributes;

                    xmlAttKey = xmlAC["key"];
                    xmlAttValue = xmlAC["value"];

                    strKey = xmlAttKey.Value;
                    strValue = xmlAttValue.Value;
                    xmlhs.Add(strKey, strValue);
                }

                if (!xmlhs.Contains(key))
                {
                    return "-1";
                }

                return xmlhs[key].ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
