<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class AddUndertakingPermissionsToUsersTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::table('users', function (Blueprint $table) {
            $table->boolean('undertaking_list')->default(false);
            $table->boolean('undertaking_add')->default(false);
            $table->boolean('undertaking_edit')->default(false);
            $table->boolean('undertaking_delete')->default(false);
            $table->boolean('undertaking_print')->default(false);
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::table('users', function (Blueprint $table) {
            $table->dropColumn([
                'undertaking_list',
                'undertaking_add',
                'undertaking_edit',
                'undertaking_delete',
                'undertaking_print'
            ]);
        });
    }
}
