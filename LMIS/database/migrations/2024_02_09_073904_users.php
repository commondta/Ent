<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class Users extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('users', function (Blueprint $table) {
            $table->id();
            $table->string('name');
            $table->string('email');
            $table->string('password');
            $table->timestamps();
            $table->boolean('isDeleted')->default(0);
            $table->string('remember_token')->default(null);
            $table->boolean('is_admin')->default(false);
            $table->boolean('lp_master_data_list')->default(false);
            $table->boolean('lp_master_data_add')->default(false);
            $table->boolean('lp_master_data_edit')->default(false);
            $table->boolean('lp_master_data_delete')->default(false);
            $table->boolean('lp_master_data_print')->default(false);
            $table->boolean('exemption_rate_list')->default(false);
            $table->boolean('exemption_rate_add')->default(false);
            $table->boolean('exemption_rate_edit')->default(false);
            $table->boolean('exemption_rate_delete')->default(false);
            $table->boolean('exemption_rate_print')->default(false);
            $table->boolean('challan_fee_list')->default(false);
            $table->boolean('challan_fee_add')->default(false);
            $table->boolean('challan_fee_edit')->default(false);
            $table->boolean('challan_fee_delete')->default(false);
            $table->boolean('challan_fee_print')->default(false);
            $table->boolean('seller_profile_list')->default(false);
            $table->boolean('seller_profile_add')->default(false);
            $table->boolean('seller_profile_edit')->default(false);
            $table->boolean('seller_profile_delete')->default(false);
            $table->boolean('seller_profile_print')->default(false);
            $table->boolean('challan_form_list')->default(false);
            $table->boolean('challan_form_add')->default(false);
            $table->boolean('challan_form_edit')->default(false);
            $table->boolean('challan_form_delete')->default(false);
            $table->boolean('challan_form_print')->default(false);
            $table->boolean('land_form_seller_list')->default(false);
            $table->boolean('land_form_seller_add')->default(false);
            $table->boolean('land_form_seller_edit')->default(false);
            $table->boolean('land_form_seller_delete')->default(false);
            $table->boolean('land_form_seller_print')->default(false);
            $table->boolean('purchase_of_land_list')->default(false);
            $table->boolean('purchase_of_land_add')->default(false);
            $table->boolean('purchase_of_land_edit')->default(false);
            $table->boolean('purchase_of_land_delete')->default(false);
            $table->boolean('purchase_of_land_print')->default(false);
            $table->boolean('possession_certificate_list')->default(false);
            $table->boolean('possession_certificate_add')->default(false);
            $table->boolean('possession_certificate_edit')->default(false);
            $table->boolean('possession_certificate_delete')->default(false);
            $table->boolean('possession_certificate_print')->default(false);
            $table->boolean('pictorial_view_list')->default(false);
            $table->boolean('pictorial_view_add')->default(false);
            $table->boolean('pictorial_view_edit')->default(false);
            $table->boolean('pictorial_view_delete')->default(false);
            $table->boolean('pictorial_view_print')->default(false);
            $table->boolean('conveyance_deed_list')->default(false);
            $table->boolean('conveyance_deed_add')->default(false);
            $table->boolean('conveyance_deed_edit')->default(false);
            $table->boolean('conveyance_deed_delete')->default(false);
            $table->boolean('conveyance_deed_print')->default(false);
            $table->boolean('agreement_list')->default(false);
            $table->boolean('agreement_add')->default(false);
            $table->boolean('agreement_edit')->default(false);
            $table->boolean('agreement_delete')->default(false);
            $table->boolean('agreement_print')->default(false);
            $table->boolean('indemnity_bond_list')->default(false);
            $table->boolean('indemnity_bond_add')->default(false);
            $table->boolean('indemnity_bond_edit')->default(false);
            $table->boolean('indemnity_bond_delete')->default(false);
            $table->boolean('indemnity_bond_print')->default(false);
            $table->boolean('registry_document_list')->default(false);
            $table->boolean('registry_document_add')->default(false);
            $table->boolean('registry_document_edit')->default(false);
            $table->boolean('registry_document_delete')->default(false);
            $table->boolean('registry_document_print')->default(false);
            $table->boolean('exemption_form_list')->default(false);
            $table->boolean('exemption_form_add')->default(false);
            $table->boolean('exemption_form_edit')->default(false);
            $table->boolean('exemption_form_delete')->default(false);
            $table->boolean('exemption_form_print')->default(false);
            $table->boolean('affidavit_2_list')->default(false);
            $table->boolean('affidavit_2_add')->default(false);
            $table->boolean('affidavit_2_edit')->default(false);
            $table->boolean('affidavit_2_delete')->default(false);
            $table->boolean('affidavit_2_print')->default(false);
            $table->boolean('intimation_application_list')->default(false);
            $table->boolean('intimation_application_add')->default(false);
            $table->boolean('intimation_application_edit')->default(false);
            $table->boolean('intimation_application_delete')->default(false);
            $table->boolean('intimation_application_print')->default(false);
            $table->boolean('intimation_letter_list')->default(false);
            $table->boolean('intimation_letter_add')->default(false);
            $table->boolean('intimation_letter_edit')->default(false);
            $table->boolean('intimation_letter_delete')->default(false);
            $table->boolean('intimation_letter_print')->default(false);
        });

    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        //
    }
}
