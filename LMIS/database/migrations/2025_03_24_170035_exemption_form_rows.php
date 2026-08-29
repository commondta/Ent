<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class ExemptionFormRows extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('exemption_form_rows', function (Blueprint $table) {
            $table->id();
            $table->integer('deed_id');
            $table->string('khewat_no');
            $table->integer('khatooni_no');
            $table->string('qatat');
            $table->string('kanal');
            $table->string('marla');
            $table->string('sq_feet');
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
