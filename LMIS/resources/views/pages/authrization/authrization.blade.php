<?php
/**
 * Created by PhpStorm.
 * User: Hafiz Umer Khan
 * Date: 1/22/2024
 * Time: 3:29 PM
 */
@extends('layouts/main')

@section('content')

    <div class="content">
        <div class="mt-4">
            <div class="row g-4">
                <div class="col-12 col-xl-12 order-1 order-xl-0">
                    <div class="mb-9">

                        <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">

                            <div class="card-body p-0">

                                <div class="p-4 code-to-copy">
                                    <h3>

                                        You Are Not Authorized to access this module. Kindly Contact with administrative.
                                    </h3>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div class="position-fixed bottom-0 end-0 p-3" style="z-index: 5">
            <div class="toast align-items-center text-white bg-dark border-0 light" id="icon-copied-toast" role="alert"
                 aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body p-3"></div>
                    <button class="btn-close btn-close-white me-2 m-auto" type="button" data-bs-dismiss="toast"
                            aria-label="Close"></button>
                </div>
            </div>
        </div>
        <footer class="footer position-absolute">
            <div class="row g-0 justify-content-between align-items-center h-100">
                <div class="col-12 col-sm-auto text-center">
                    <p class="mb-0 mt-2 mt-sm-0 lm-footer-text"><span class="lm-footer-brand">Land Information Management System</span><span class="lm-footer-sep">|</span><span>&copy; {{ date('Y') }}</span><span class="lm-footer-sep">|</span><span>Powered by <img src="{{ asset('public/assets/img/n-stack-logo.png') }}" alt="" class="lm-footer-logo"> <strong>N-Stack</strong></span></p>
                </div>
                <div class="col-12 col-sm-auto text-center">
                </div>
            </div>
        </footer>
    </div>

    <script>
        function confirmSubmit() {
            var confirmed = window.confirm("Are you sure you want to Delete this record");
            if (confirmed) {
                // If the user confirms, submit the form
                document.getElementById("deleteForm").submit();
            }
        }
    </script>
    <!-- Your content here -->
@endsection