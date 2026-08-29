<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class LandForm extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('Land_forms', function (Blueprint $table) {
            $table->id();
            $table->string('lo_cod');
            $table->string('doc_date');
            $table->string('doc_no');
//            $table->string('lo_code');
            $table->string('lo_cnic');
            $table->string('lo_name');
            $table->string('lo_cnic_issue_date');
            $table->string('so');
            $table->string('area');
            $table->string('mouza');
            $table->string('contact_no');
            $table->string('tehsil');
            $table->string('kanal');
            $table->string('district');
            $table->string('sq_feet');
            $table->string('address');
            $table->string('marla');
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
