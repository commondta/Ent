<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class DocumentApproval extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('document_approvals', function (Blueprint $table) {
            $table->id();
            $table->string('document_name');
            $table->integer('document_id');
            $table->integer('approval_user_id');
            $table->integer('status')->default(0);
            $table->integer('priority')->default(0);
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
