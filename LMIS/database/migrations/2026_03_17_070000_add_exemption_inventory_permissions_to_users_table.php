<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class AddExemptionInventoryPermissionsToUsersTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::table('users', function (Blueprint $table) {
            $table->boolean('exemption_inventory_list')->default(false)->after('intimation_letter_print');
            $table->boolean('exemption_inventory_add')->default(false)->after('exemption_inventory_list');
            $table->boolean('exemption_inventory_edit')->default(false)->after('exemption_inventory_add');
            $table->boolean('exemption_inventory_delete')->default(false)->after('exemption_inventory_edit');
            $table->boolean('exemption_inventory_print')->default(false)->after('exemption_inventory_delete');
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
                'exemption_inventory_list',
                'exemption_inventory_add',
                'exemption_inventory_edit',
                'exemption_inventory_delete',
                'exemption_inventory_print',
            ]);
        });
    }
}
