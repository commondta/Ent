<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class IntApplication extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('int_applications', function (Blueprint $table) {
            $table->id();
            $table->string('file_no');
            $table->string('doc_no');
            $table->date('date');
            $table->string('lo_code');
            $table->string('lo_name');
            $table->string('lo_cnic');
            $table->string('lo_father_name');
            $table->string('attachment');
            $table->string('code_no');
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
