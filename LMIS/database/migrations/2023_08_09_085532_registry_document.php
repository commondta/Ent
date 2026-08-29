<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class RegistryDocument extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('registry_documents', function (Blueprint $table) {
            $table->id();
            $table->string('doc_no');
            $table->date('date');
            $table->string('lo_name');
            $table->string('lo_father_name');
            $table->string('lo_cnic');
            $table->string('indemnity_bond')->default(null);
            $table->string('agreement')->default(null);
            $table->string('undertaking')->default(null);
            $table->string('afidavit')->default(null);
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
