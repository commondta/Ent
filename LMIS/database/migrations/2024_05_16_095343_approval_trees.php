<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class ApprovalTrees extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('approval_trees', function (Blueprint $table) {
            $table->id();
            $table->integer('lp_master_data')->default(0);
            $table->integer('exemption_rate')->default(0);
            $table->integer('challan_fee')->default(0);
            $table->integer('seller_profile')->default(0);
            $table->integer('land_form_seller')->default(0);
            $table->integer('purchase_of_land')->default(0);
            $table->integer('possession_certificate')->default(0);
            $table->integer('pictorial_view')->default(0);
            $table->integer('conveyance_deed')->default(0);
            $table->integer('agreement')->default(0);
            $table->integer('indemnity_bond')->default(0);
            $table->integer('registry_document')->default(0);
            $table->integer('exemption_form')->default(0);
            $table->integer('affidavit_2')->default(0);
            $table->integer('intimation_application')->default(0);
            $table->integer('intimation_letter')->default(0);
            $table->boolean('isDeleted')->default(0);
            $table->timestamps();
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
