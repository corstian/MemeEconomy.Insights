![](https://styles.redditmedia.com/t5_3gl3k/styles/bannerPositionedImage_3it9rpm9xab21.png)

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/1d4f0422e86140949f0701f2ef2fb890)](https://www.codacy.com/app/corstian/MemeEconomy.Insights?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=corstian/MemeEconomy.Insights&amp;utm_campaign=Badge_Grade)

MemeEconomy.Insights
====================

Follow the latest and greatest market trends! This application enables you to invest in the newest and most innovative memes on the market.

Just kidding. Read on.


## Introduction

MemeEconomy.Insights is a mashup between asp.net core, graphql-dotnet, and RedditSharp. Combining these three ingredients we get an environment with which we can extract the latest trends from /r/memeeconomy, and expose it easily through our GraphQL api for use in some front-end application.

## Services

This application depends on a single service which checks Reddit for new content.

- **MemeEconomyStalker**  
  *This service is registered as `IHostedService` so it is ran on startup. All it does is follow "MemeInvestor_bot" for comments. With some string extraction magic we get to know about what happens on the trade floor.*

## GraphQL

GraphQL is being used as our outlet to the real world. One can use queries for historic data, and subscriptions for realtime information. Feel free to use your favorite GraphQL client in order to access this data.

## Terms of Service

You're totally free to use this api as you like. You may use the hosted version on [...]() to link your web-application or whatever. Note there is an IP based rate-limiter in place which gives all of you an equal change to use this api. Last but not least; availability of this api is subject to my own monetary and computational abilities.

[Help me exchange memecoins for computational power.](https://www.paypal.me/corstianboerman)

---

*Happy trading!*

![](https://media.giphy.com/media/94EQmVHkveNck/giphy.gif)


***BTW:** This service has originally been built to serve as an example to show how to use the graphql-dotnet library. Do as you wish with this information.*