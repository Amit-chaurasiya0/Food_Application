// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let apiURL = "https://forkify-api.herokuapp.com/api/v2/recipes";
//let apiKey = "9a940059-5558-45d7-a35a-22cc79f21b93";
let apiKey = "76e66ddd-dcbd-4096-8e6b-9cecb381455d";
async function GetRecipes(recipeName, id, isAllShow) {
    let resp = await fetch(`${apiURL}?search=${recipeName}&key=${apiKey}`);
    let result = await resp.json();
    let Recipes = isAllShow ? result.data.recipes : result.data.recipes.slice(0, 7);
    ShowRecipes(Recipes, id);
    
}

function ShowRecipes(recipes, id) {
    $.ajax({
        contentType: "application/json; charset=utf-8",
        datatype: 'html',
        type: 'POST',
        url: '/Recipe/GetRecipeCard',
        data: JSON.stringify(recipes),
        success: function (htmlResult) {
             
            $('#' + id).html(htmlResult)
            getAddedCart();
        }
    })
}

async function getOrderRecipe(id,showId) {
    let resp = await fetch(`${apiURL}/${id}?key=${apiKey}`);
    let result = await resp.json();
    console.log(result);
    let recipe = result.data.recipe;
    showOrderRecipeDetails(recipe, showId);
}
function showOrderRecipeDetails(orderRecipeDetails, showId) {
    $.ajax({        
        url: '/Recipe/ShowOrder',
        data: orderRecipeDetails,
        datatype: 'html',
        type: 'POST',               
        success: function (htmlResult) {
            $('#' + showId).html(htmlResult)
        }
    })
}

    //order page

function quantity(option) {
    let qty = parseInt($('#Qty').val());
    let price = parseInt($('#price').val());
    let TotalPrice = 0;
    if (option === 'inc') {
        qty = qty + 1;
    } else {
        qty = qty == 1 ? qty : qty - 1;
    }
    TotalPrice = price * qty;
    $('#Qty').val(qty);
    $('#TotalAmount').val(TotalPrice);
}

    // Add to Cart

async function cart() {
    let itag = $(this).children('i')[0];    
    let recipeId = $(this).attr('data-recipeId');    
    if ($(itag).hasClass('fa-regular')) {
        let resp = await fetch(`${apiURL}/${recipeId}?key=${apiKey}`);
        let result = await resp.json();
        let cart = result.data.recipe;
        cart.recipeId = recipeId;
        delete cart.id;
        cartRequest(cart,'SaveCart','fa-solid','fa-regular',itag,false);
    }
    else {
        let data = { id: recipeId }
        cartRequest(data, 'RemoveCartFromList', 'fa-regular', 'fa-solid', itag,false)
    }
}
function cartRequest(cart, action, Addcls, Removecls, itag, isReload) {
    //console.log(data);
    $.ajax({
        url: '/Cart/' + action,
        type: 'POST',
        data: cart,
        success: function (resp) {
            if (isReload) {
                location.reload();
            }else {
                $(itag).addClass(Addcls);
                $(itag).removeClass(Removecls);
            }            
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function getAddedCart() {
    $.ajax({
        url: '/Cart/GetAddedCart',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            //console.log(result);
            $('.addToCartIcon').each((index, spantag) => {
                let recipeId = $(spantag).attr("data-recipeId");
                for (var i = 0; i < result.length; i++) {
                    if (recipeId == result[i]) {
                        let itag = $(spantag).children('i')[0];
                        $(itag).addClass('fa-solid');
                        $(itag).removeClass('fa-regular');
                        break;
                    }
                }
            });
        },
        error: function (err) {
            console.log(err);
        }
    });
}
function getCartList() {
    $.ajax({
        url: '/Cart/GetCartList',
        type: 'GET',
        dataType: 'html',
        success: function (result) {
            $('#showCartList').html(result);
        },
        error: function (err) {
            console.log(err)
        }
    });
}
function removeCartFromList(id) {
    console.log(id);
    let data = { Id: id };
    cartRequest(data, 'RemoveCartFromList', null, null, null, true);
}
