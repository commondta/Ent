<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class IntimationLetter extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {

        Schema::create('intimation_letters', function (Blueprint $table) {
            $table->id();
            $table->string('doc_no');
            $table->date('date');
            $table->string('application_no');
            $table->string('file_no');
            $table->string('code_no');
            $table->string('lo_code');
            $table->string('lo_name');
            $table->string('lo_address');
            $table->string('lo_father_name');
            $table->string('purchaser');
            $table->string('purchaser_address');
            $table->string('purchaser_cnic');
            $table->string('district');
            $table->string('tehsil');
            $table->string('lp_name');
            $table->string('lp_father_name');
            $table->string('affidavit_no');
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
