@extends('layouts.main')

@section('content')
    <div class="content">
        <div class="mt-4">
            <div class="row g-4">
                <div class="col-12 col-xl-12 order-1 order-xl-0">
                    <div class="mb-9">
                        <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                            <div class="card-header p-4 border-bottom border-300 bg-soft">
                                <div class="row g-3 justify-content-between align-items-center">
                                    <div class="col-12 col-md">
                                        <h4 class="text-900 mb-0" data-anchor="data-anchor">Pictorial View</h4>
                                    </div>

                                </div>
                            </div>
                            <div class="card-body p-0">

                                <div class="p-4 code-to-copy">
                                    @if(session('status'))
                                        <div class="alert alert-success mb-1 mt-1">
                                            {{ session('status') }}
                                        </div>
                                    @endif
                                    <form class="row g-3 needs-validation" method="post" action="{{ route('pictorial_view.update',$pictorial_view->id) }}" novalidate=""  enctype="multipart/form-data">
                                        @csrf
                                        @method('PUT')
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row">


                                                    <div class="col-md-6">
                                                        <label class="form-label" for="doc_no">Doc No.</label>
                                                        <input class="form-control" id="doc_no" type="text" name="doc_no" readonly value="{{ $pictorial_view->doc_no }}" required="" />
                                                        <div class="valid-feedback">Please Add Doc No.</div>
                                                        @error('doc_no')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>

                                                    <div class="col-md-6">
                                                        <label class="form-pc_no" for="pc_no">Possession Certificate No</label>
                                                        <select id="mySelect" name="pc_no" class="form-control"
                                                                required="">
                                                            <option value="">Kindly Select</option>
                                                            @foreach($possession_certificate as $row)
                                                                <option @if($pictorial_view->pc_no == $row->doc_no ) selected @endif value="{{ $row->doc_no }}">Possession Certificate No - {{ $row->doc_no }}</option>
                                                            @endforeach
                                                        </select>

                                                        <div class="invalid-feedback">Please add Possession Certificate No</div>
                                                        @error('pc_no')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="form-label" for="lo_name">LO Name</label>
                                                        <input class="form-control" id="lo_name" type="text" name="lo_name" value="{{ $pictorial_view->lo_name }}" readonly required="" />
                                                        <div class="invalid-feedback">Please add LO Name.</div>
                                                        @error('lo_name')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="form-label" for="chak">Chak</label>
                                                        <input class="form-control" id="chak" type="text" name="chak" value="{{ $pictorial_view->chak }}" readonly required="" />
                                                        <div class="invalid-feedback">Please add Chak.</div>
                                                        @error('chak')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="form-label" for="lp_name">LP Name</label>
                                                        <input class="form-control" id="lp_name" type="text" name="lp_name" value="{{ $pictorial_view->lp_name }}" readonly required="" />
                                                        <div class="invalid-feedback">Please add LP Name.</div>
                                                        @error('lp_name')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="form-label" for="area">Area</label>
                                                        <input class="form-control" id="area" type="text" name="area" value="{{ $pictorial_view->area }}"  />
                                                        <div class="invalid-feedback">Please add Area.</div>
                                                        @error('area')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="form-label" for="kanal">Kanal</label>
                                                        <input class="form-control" id="kanal" type="text" name="kanal" value="{{ $pictorial_view->kanal }}" readonly required="" />
                                                        <div class="invalid-feedback">Please add Kanal.</div>
                                                        @error('kanal')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="form-label" for="marla">Marla</label>
                                                        <input class="form-control" id="marla" type="text" name="marla" value="{{ $pictorial_view->marla }}" readonly required="" />
                                                        <div class="invalid-feedback">Please add Marla.</div>
                                                        @error('marla')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="form-label" for="name_of_patwari">Name Of Patwari</label>
                                                        <input class="form-control" id="name_of_patwari" type="text" name="name_of_patwari" value="{{ $pictorial_view->name_of_patwari }}"  required="" />
                                                        <div class="invalid-feedback">Please add Patwari Name.</div>
                                                        @error('name_of_patwari')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="form-label" for="signature1">Signature</label>
                                                        <input class="form-control" id="signature1" type="text" name="signature1" value="{{ $pictorial_view->signature1 }}"  required="" />
                                                        <div class="invalid-feedback">Please add Signature.</div>
                                                        @error('signature1')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label class="form-label" for="possession_jco">Possession JCO</label>
                                                        <input class="form-control" id="possession_jco" type="text" name="possession_jco" value="{{ $pictorial_view->possession_jco }}"  required="" />
                                                        <div class="invalid-feedback">Please add Possession JCO.</div>
                                                        @error('possession_jco')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>

                                                    <div class="col-md-3">
                                                        <label class="form-label" for="signature2">Signature</label>
                                                        <input class="form-control" id="signature2" type="text" name="signature2" value="{{ $pictorial_view->signature2 }}"  required="" />
                                                        <div class="invalid-feedback">Please add Signature.</div>
                                                        @error('signature2')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>

                                                    <div class="col-md-4">

                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="picture">Attachment</label>
                                                            <input class="form-control"
                                                                   id="picture"
                                                                   name="picture"
                                                                   type="file"
                                                                   value="{{ $pictorial_view->picture }}"
                                                                   />
                                                            @if($pictorial_view->picture)
                                                                <?php
                                                                $filename = $pictorial_view->picture;
                                                                $extension = pathinfo($filename, PATHINFO_EXTENSION);

                                                                ?>
                                                                @if(in_array($extension, ['jpg', 'jpeg', 'png', 'gif']))
                                                                    <img src="{{ URL::asset('public/assets/uploads/').'/'.$pictorial_view->picture; }}"
                                                                         style="width: 294px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                @else
                                                                    <a target="_blank"
                                                                       href="{{ URL::asset('public/assets/uploads/').'/'.$pictorial_view->picture; }}">
                                                                        <img src="{{ URL::asset('public/assets/').'/'.'file.png'; }}"
                                                                             style="width: 294px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                    </a>
                                                                @endif
{{--                                                                <img src="{{ URL::asset('public/assets/uploads/').'/'.$pictorial_view->picture; }}" style="width: 295px;  border: 1px solid #CBD0DD;  border-radius: 4px;">--}}
                                                            @endif
                                                        </div>

                                                    </div>
                                                </div>


                                            </div>
                                        </div>


                                        <div class="col-12">
                                            <button class="btn btn-primary" type="submit">Submit form</button>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div class="position-fixed bottom-0 end-0 p-3" style="z-index: 5">
            <div class="toast align-items-center text-white bg-dark border-0 light" id="icon-copied-toast" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body p-3"></div><button class="btn-close btn-close-white me-2 m-auto" type="button" data-bs-dismiss="toast" aria-label="Close"></button>
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


            $('#mySelect').change(function() {
                var selectedValue = $(this).val();

                $.ajax({
                    url: baseUrl+'/get_possession_record',
                    type: 'POST', // or 'GET', 'PUT', 'DELETE', etc. depending on your API
                    data: JSON.stringify({"_token": "{{ csrf_token() }}", value: selectedValue }), // You can send data to the server if required
                    contentType: 'application/json', // Set the appropriate content type
                    success: function(data) {

                        console.log(data);
                        $('#lo_name').val(data.owner_Name);
                        $('#lp_name').val(data.provider_Name);
                        $('#chak').val(data.chak);
                        $('#kanal').val(data.kanal);
                        $('#marla').val(data.marla);


//                        $('#lo_name').prop('readonly', true);
//                        $('#lp_name').prop('readonly', true);
//                        $('#chak').prop('readonly', true);
//                        $('#kanal').prop('readonly', true);
//                        $('#marla').prop('readonly', true);
                        // Do something with the data (e.g., update content on the page)
                    },
                    error: function(error) {
                        // Handle any errors that occurred during the AJAX call
                        console.error('Error:', error);
                    }
                });
            });

    </script>


@endsection