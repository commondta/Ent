<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;
use Illuminate\Support\Facades\DB;

class ModifyLpNameColumn extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        // Modify lp_name column to JSON type for storing multiple values
        DB::statement('SET FOREIGN_KEY_CHECKS=0');
        DB::statement('ALTER TABLE `purchase_of_lands` DROP COLUMN `lp_name`');
        DB::statement('ALTER TABLE `purchase_of_lands` ADD COLUMN `lp_name` JSON NULL AFTER `lo_name`');
        DB::statement('SET FOREIGN_KEY_CHECKS=1');
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        // Revert back to string if needed
        DB::statement('SET FOREIGN_KEY_CHECKS=0');
        DB::statement('ALTER TABLE `purchase_of_lands` DROP COLUMN `lp_name`');
        DB::statement('ALTER TABLE `purchase_of_lands` ADD COLUMN `lp_name` VARCHAR(255) NULL AFTER `lo_name`');
        DB::statement('SET FOREIGN_KEY_CHECKS=1');
    }
}
