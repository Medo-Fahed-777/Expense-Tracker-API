using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ExpenseTracker.Extensions
{
    public static class CustomJwtAuth
    {
         public static void AddCustomJwtAuth
        (this IServiceCollection services, ConfigurationManager config)
        {
            services.AddAuthentication(
               op =>
                {
                    op.DefaultAuthenticateScheme =
                    op.DefaultChallengeScheme =
                    op.DefaultForbidScheme =
                    op.DefaultScheme =
                    op.DefaultSignInScheme =
                    op.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(
                op =>
                    {
                        op.TokenValidationParameters = new()
                        {
                            ValidateIssuer = true,
                            ValidIssuer = config["JWT:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = config["JWT:Audience"],
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SigningKey"] ??
                            throw new InvalidOperationException("JWT:SigningKey is not configured."))),
                              NameClaimType = JwtRegisteredClaimNames.Sub, // 👈 maps "sub" to user identity
                              RoleClaimType = ClaimTypes.Role
                        };
                    });
        }

        public static void AddSwaggerGenJwtAuth( this IServiceCollection services )
        {
            services.AddSwaggerGen(
                op =>
                {
                    op.SwaggerDoc("v1", new OpenApiInfo()
                    {
                        Version = "v1",
                        Title = "Web Api Test",
                        Contact = new OpenApiContact()
                        {
                            Name = "Moayyad Al Fahed",
                            Email = "moaiadefahed2000@gmali.com",
                        },
                    });
                    op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Paste your JWT token below (no 'Bearer' prefix needed)."
                    }
                    );
                    op.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                }
            );
        }
    }
    }
