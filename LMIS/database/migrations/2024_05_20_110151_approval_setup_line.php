<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class ApprovalSetupLine extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('approval_setup_lines', function (Blueprint $table) {
            $table->id();
            $table->integer('user');
            $table->integer('main');
            $table->integer('designation');
            $table->integer('priority');
            $table->integer('status')->default(0);
            $table->string('remarks')->default(null);
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
