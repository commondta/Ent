<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateLandFormRowTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('land_form_rows', function (Blueprint $table) {
            $table->id();
            $table->foreignId('land_form_id')->constrained('Land_forms')->onDelete('cascade');
            $table->string('lo_cod');
            $table->string('lo_name')->nullable();
            $table->string('lo_cnic')->nullable();
            $table->string('contact_no')->nullable();
            $table->string('so')->nullable();
            $table->string('mouza')->nullable();
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
        Schema::dropIfExists('land_form_rows');
    }
}
