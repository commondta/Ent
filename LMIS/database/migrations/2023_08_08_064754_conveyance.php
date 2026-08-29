<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class Conveyance extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('conveyances', function (Blueprint $table) {
            $table->id();
            $table->string('doc_no');
            $table->date('date');
            $table->integer('base_doc_no');
            $table->string('date_of_creation');
            $table->string('lo_name');
            $table->string('lo_cnic');
            $table->string('lo_address');
            $table->string('lo_khewat')->default(null);
            $table->string('tehsil')->default(null);
            $table->string('khatooni')->default(null);
            $table->string('scheme')->default(null);
            $table->string('qatat')->default(null);
            $table->string('fixed_deed_rs');
            $table->string('stamp_paper_value');
            $table->string('transferred_share');
            $table->string('schedule_year');
            $table->string('vide_fad_id_no');
            $table->string('chak_no');
            $table->string('deed_executed_by_lo_name');
            $table->string('deed_executed_by_lo_father_name')->default('');
            $table->string('deed_executed_by_cnic');
            $table->string('deed_executed_by_caste');
            $table->string('deed_executed_by_address');
            $table->string('deed_in_favor_of_name');
            $table->string('deed_in_favor_of_principal_office');
            $table->string('deed_in_favor_of_project_office');
            $table->string('deed_in_favor_of_representative');
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
