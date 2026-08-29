<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class Agreement extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('agreements', function (Blueprint $table) {
            $table->id();
            $table->string('doc_no');
            $table->date('date');
            $table->integer('base_doc_no');
            $table->string('agreement_date');
            $table->string('lo_name');
            $table->string('lo_father_name');
            $table->string('lo_cnic');
            $table->string('lo_caste');
            $table->string('lo_address');
            $table->string('tehsil');
            $table->string('chak');
            $table->string('b_name');
            $table->string('b_principle_office');
            $table->string('b_project_office');
            $table->string('b_representative');
            $table->string('khewat');
            $table->string('khatooni');
            $table->string('qatat');
            $table->string('area');
            $table->string('transfer_share');
            $table->string('year_of_ROR');
            $table->string('vide_fad_id_no');
            $table->string('fard_id_date');
            $table->string('chak_no');
            $table->string('tehsil_no');
            $table->string('district_no');
            $table->string('misc_charges_file');
            $table->integer('status')->default(1);
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
