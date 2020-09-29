const admin = require("firebase-admin");

admin.initializeApp({
  databaseURL: DATABASE_URL,
});

const functions = require("firebase-functions");

exports.get_content = functions.https.onRequest((request, response) => {
  const uid = request.query.uid;

  admin
    .auth()
    .getUser(uid)
    .then(() => {
      admin
        .database()
        .ref()
        .child("content")
        .once("value")
        .then((snapshot) => {
          var categories = [
            [
              "Relaxed",
              "Cannabis can relax both the body and the mind when looking to wind down at the end of a long day.",
            ],
            [
              "Hungry",
              "Grumbles in your tummy? These strains can help naturally kick-start appetite.",
            ],
            [
              "Happy",
              "It's said that true happiness is a state of mind. These strains can help induce intense feelings of joy.",
            ],
            [
              "Creative",
              "Expand your imagination with strains that can stimulate new creative thoughts and ideas.",
            ],
            ["Energetic", "Keep active with strains that can crush fatigue."],
            [
              "Social",
              "Liven up social events with strains that can help reduce social anxiety and provide talkative highs.",
            ],
            [
              "Sleepy",
              "Sleep is essential for maintaining health. Cannabis can be an effective treatment for sleep disorders and difficulties.",
            ],
            [
              "Focused",
              "Get more done faster with strains that can improve focus.",
            ],
            [
              "Giggly",
              "Find the perfect companion strain to enjoy with your favorite music, movies and shows.",
            ],
            [
              "Aroused",
              "Bring some bud into the bedroom with strains that can heighten sexual experiences.",
            ],
          ];

          var items = [];

          snapshot.forEach(function (item) {
            items.push(item);
          });

          var functions = {
            vibe: function (shuffledItems, targetEffect) {
              var itemsToLoad = [];

              for (var item of shuffledItems) {
                var effects = item.child("effects").child("positive").val();

                if (effects != null) {
                  effects.forEach(function (effect) {
                    if (effect == targetEffect) {
                      itemsToLoad.push(item);
                    }
                  });

                  if (itemsToLoad.length == 10) {
                    return itemsToLoad;
                  }
                }
              }

              return null;
            },
          };

          var curatedList = [];

          for (var i = 0; i < 7; i++) {
            var shuffledItems = shuffle(items);

            var index = Math.floor(Math.random() * categories.length);
            var category = categories[index];
            categories.splice(index, 1);

            if (
              category[0] == "Relaxed" ||
              category[0] == "Hungry" ||
              category[0] == "Happy" ||
              category[0] == "Creative" ||
              category[0] == "Energetic" ||
              category[0] == "Sleepy" ||
              category[0] == "Focused" ||
              category[0] == "Giggly" ||
              category[0] == "Aroused"
            ) {
              itemsToLoad = functions.vibe(shuffledItems, category[0]);
            } else if (category[0] == "Social") {
              itemsToLoad = functions.vibe(shuffledItems, "Talkative");
            }

            if (itemsToLoad != null) {
              var obj = new Object();
              obj.title = category[0];
              obj.description = category[1];
              obj.items = itemsToLoad;

              curatedList.push(obj);
            }
          }

          var ratedCuratedItems = [];

          curatedList.forEach(function (obj) {
            obj.items.forEach(function (item) {
              ratedCuratedItems.push(getReviews(item));
            });
          });

          var featuredList = [];
          var shuffledItems = shuffle(items);

          for (var i = 0; i < 3; i++) {
            var index = Math.floor(Math.random() * shuffledItems.length);
            var featuredItem = shuffledItems[index];
            shuffledItems.splice(index, 1);
            featuredList.push(featuredItem);
          }

          var ratedFeaturedList = [];

          featuredList.forEach(function (item) {
            ratedFeaturedList.push(getReviews(item));
          });

          Promise.all(ratedCuratedItems).then(function (ratedCuratedItems) {
            Promise.all(ratedFeaturedList).then(function (ratedFeaturedList) {
              var ratedCuratedList = [];

              for (var i = 0; i < 7; i++) {
                var obj = new Object();
                obj.title = curatedList[i].title;
                obj.description = curatedList[i].description;

                var items = [];

                for (var j = 0; j < 10; j++) {
                  items.push(ratedCuratedItems[i * 10 + j]);
                }

                obj.items = items;

                ratedCuratedList.push(obj);
              }

              response.json({
                curated: ratedCuratedList,
                featured: ratedFeaturedList,
              });
            });
          });
        });
    });
});

exports.search = functions.https.onRequest((request, response) => {
  const uid = request.query.uid;

  admin
    .auth()
    .getUser(uid)
    .then(() => {
      var obj = JSON.parse(request.body);
      var words = obj.words;

      admin
        .database()
        .ref()
        .child("content")
        .once("value")
        .then((snapshot) => {
          var items = [];

          snapshot.forEach(function (item) {
            items.push(item);
          });

          var results = [];

          shuffle(items).forEach(function (item) {
            var match = false;
            var priority = 0;

            for (var word of words) {
              if (
                item
                  .child("name")
                  .val()
                  .toLowerCase()
                  .includes(word.toLowerCase())
              ) {
                match = true;
                priority += 10;
              }

              var flavors = item.child("flavors").val();
              if (flavors != null) {
                flavors.forEach(function (flavor) {
                  if (flavor.toLowerCase().includes(word.toLowerCase())) {
                    match = true;
                    priority += 3;
                  }
                });
              }

              var positiveEffects = item
                .child("effects")
                .child("positive")
                .val();
              if (positiveEffects != null) {
                positiveEffects.forEach(function (effect) {
                  if (effect.toLowerCase().includes(word.toLowerCase())) {
                    match = true;
                    priority += 3;
                  }
                });
              }

              var negativeEffects = item
                .child("effects")
                .child("negative")
                .val();
              if (negativeEffects != null) {
                negativeEffects.forEach(function (effect) {
                  if (effect.toLowerCase().includes(word.toLowerCase())) {
                    match = true;
                    priority += 3;
                  }
                });
              }

              var medicalEffects = item.child("effects").child("medical").val();
              if (medicalEffects != null) {
                medicalEffects.forEach(function (effect) {
                  if (effect.toLowerCase().includes(word.toLowerCase())) {
                    match = true;
                    priority += 3;
                  }
                });
              }
            }

            if (match) {
              results.push([getReviews(item), priority]);
            }
          });

          results.sort(function (a, b) {
            return b[1] - a[1];
          });

          results = results.slice(0, 100);

          var firstHundredResults = results.map(function (result) {
            return result[0];
          });

          Promise.all(firstHundredResults).then(function (firstHundredResults) {
            response.json({ result: firstHundredResults });
          });
        });
    });
});

exports.highfive = functions.https.onRequest((request, response) => {
  const uid = request.query.uid;

  admin
    .auth()
    .getUser(uid)
    .then(() => {
      admin
        .database()
        .ref()
        .child("users")
        .child(uid)
        .once("value")
        .then((user) => {
          var topics = [];
          var tags = [];

          if (user.child("topics").val() != null) {
            user.child("topics").forEach(function (topic) {
              topics.push(topic);
            });
          }

          if (user.child("tags").val() != null) {
            user.child("tags").forEach(function (tag) {
              tags.push(tag);
            });
          }

          admin
            .database()
            .ref()
            .child("content")
            .once("value")
            .then((content) => {
              var items = [];

              content.forEach(function (item) {
                items.push(item);
              });

              items = shuffle(items);

              var rankings = [];

              items.forEach(function (item) {
                var num = 0;
                var den = 0;

                for (var topic of topics) {
                  var match = false;

                  var flavors = item.child("flavors").val();
                  if (flavors != null) {
                    flavors.forEach(function (flavor) {
                      if (
                        flavor.toLowerCase().includes(topic.key.toLowerCase())
                      ) {
                        match = true;
                      }
                    });
                  }

                  var positiveEffects = item
                    .child("effects")
                    .child("positive")
                    .val();
                  if (positiveEffects != null) {
                    positiveEffects.forEach(function (effect) {
                      if (
                        effect.toLowerCase().includes(topic.key.toLowerCase())
                      ) {
                        match = true;
                      }
                    });
                  }

                  var medicalEffects = item
                    .child("effects")
                    .child("medical")
                    .val();
                  if (medicalEffects != null) {
                    medicalEffects.forEach(function (effect) {
                      if (
                        effect.toLowerCase().includes(topic.key.toLowerCase())
                      ) {
                        match = true;
                      }
                    });
                  }

                  var worth = topic.val() * 50;

                  if (match) {
                    num += worth;
                    den += worth;
                  } else {
                    num += 0.8 * worth;
                    den += worth;
                  }
                }

                for (var tag of tags) {
                  var match = false;

                  var flavors = item.child("flavors").val();
                  if (flavors != null) {
                    flavors.forEach(function (flavor) {
                      if (
                        flavor.toLowerCase().includes(tag.key.toLowerCase())
                      ) {
                        match = true;
                      }
                    });
                  }

                  var positiveEffects = item
                    .child("effects")
                    .child("positive")
                    .val();
                  if (positiveEffects != null) {
                    positiveEffects.forEach(function (effect) {
                      if (
                        effect.toLowerCase().includes(tag.key.toLowerCase())
                      ) {
                        match = true;
                      }
                    });
                  }

                  var medicalEffects = item
                    .child("effects")
                    .child("medical")
                    .val();
                  if (medicalEffects != null) {
                    medicalEffects.forEach(function (effect) {
                      if (
                        effect.toLowerCase().includes(tag.key.toLowerCase())
                      ) {
                        match = true;
                      }
                    });
                  }

                  var worth = tag.val() * 1;

                  if (match) {
                    num += worth;
                    den += worth;
                  } else {
                    num += 0.8 * worth;
                    den += worth;
                  }
                }

                if (den != 0) {
                  rankings.push([getReviews(item), num / den]);
                } else {
                  rankings.push([getReviews(item), 0]);
                }
              });

              rankings.sort(function (a, b) {
                return b[1] - a[1];
              });

              rankings = rankings.slice(0, 5);

              Promise.all(
                rankings.map(function (ranking) {
                  return ranking[0];
                })
              ).then(function (resolved) {
                var result = [];

                for (var i = 0; i < 5; i++) {
                  var obj = new Object();
                  obj.item = resolved[i];
                  obj.matchPercent = rankings[i][1];

                  result.push(obj);
                }

                response.json({ result });
              });
            });
        });
    });
});

exports.get_reviews = functions.https.onRequest((request, response) => {
  const strain_id = request.query.strain_id;
  const uid = request.query.uid;

  admin
    .auth()
    .getUser(uid)
    .then(() => {
      admin
        .database()
        .ref()
        .child("reviews")
        .child(strain_id)
        .once("value")
        .then((snapshot) => {
          if (snapshot.exists()) {
            admin
              .database()
              .ref()
              .child("reviews")
              .child(strain_id)
              .child("data")
              .limitToLast(25)
              .once("value")
              .then((ratings) => {
                var averageRating = snapshot.child("averageRating").val();
                var numberOfReviews = snapshot.child("numberOfReviews").val();
                var ratings = ratings.val();

                response.json({
                  averageRating: averageRating,
                  numberOfReviews: numberOfReviews,
                  ratings: ratings,
                });
              });
          } else {
            response.json({
              averageRating: 0,
              numberOfReviews: 0,
              ratings: {},
            });
          }
        });
    });
});

exports.post_review = functions.https.onRequest((request, response) => {
  const uid = request.query.uid;

  admin
    .auth()
    .getUser(uid)
    .then(() => {
      var obj = JSON.parse(JSON.stringify(request.body));

      admin
        .database()
        .ref()
        .child("reviews")
        .child(obj.id)
        .once("value")
        .then((snapshot) => {
          var date = new Date();

          admin
            .database()
            .ref()
            .child("users")
            .child(uid)
            .once("value")
            .then((snapshot) => {
              var name = snapshot.child("name").val();

              admin
                .database()
                .ref()
                .child("reviews")
                .child(obj.id)
                .child("data")
                .push({
                  poster_name: name,
                  score: parseFloat(obj.rating),
                  review: obj.review,
                  timestamp:
                    "" +
                    (date.getMonth() + 1) +
                    "/" +
                    date.getDate() +
                    "/" +
                    date.getFullYear(),
                })
                .then((snapshot) => {
                  admin
                    .database()
                    .ref()
                    .child("users")
                    .child(uid)
                    .child("reviews")
                    .push({
                      review_key: snapshot.key,
                      strain_id: obj.id,
                      timestamp:
                        "" +
                        (date.getMonth() + 1) +
                        "/" +
                        date.getDate() +
                        "/" +
                        date.getFullYear(),
                    });
                });
            });

          if (snapshot.exists()) {
            var averageRating = snapshot.child("averageRating").val();
            var numberOfReviews = snapshot.child("numberOfReviews").val();

            admin
              .database()
              .ref()
              .child("reviews")
              .child(obj.id)
              .update({
                averageRating:
                  (averageRating * numberOfReviews + parseFloat(obj.rating)) /
                  (numberOfReviews + 1),
                numberOfReviews: numberOfReviews + 1,
              });
          } else {
            admin
              .database()
              .ref()
              .child("reviews")
              .child(obj.id)
              .update({
                averageRating: parseFloat(obj.rating),
                numberOfReviews: 1,
              });
          }

          response.send("Success");
        });
    });
});

exports.update_logs = functions.https.onRequest((request, response) => {
  const uid = request.query.uid;

  admin
    .auth()
    .getUser(uid)
    .then(() => {
      var obj = JSON.parse(request.body);
      var topics = obj.Topics;
      var tags = obj.Tags;

      admin
        .database()
        .ref()
        .child("users")
        .child(uid)
        .once("value")
        .then((snapshot) => {
          for (var key in topics) {
            if (snapshot.child("topics").child(key).val() != null) {
              admin
                .database()
                .ref()
                .child("users")
                .child(uid)
                .child("topics")
                .update({
                  [key]:
                    snapshot.child("topics").child(key).val() + topics[key],
                });
            } else {
              admin
                .database()
                .ref()
                .child("users")
                .child(uid)
                .child("topics")
                .update({
                  [key]: topics[key],
                });
            }
          }

          for (var key in tags) {
            if (snapshot.child("tags").child(key).val() != null) {
              admin
                .database()
                .ref()
                .child("users")
                .child(uid)
                .child("tags")
                .update({
                  [key]: snapshot.child("tags").child(key).val() + tags[key],
                });
            } else {
              admin
                .database()
                .ref()
                .child("users")
                .child(uid)
                .child("tags")
                .update({
                  [key]: tags[key],
                });
            }
          }
        });

      response.send("Success");
    });
});

function getReviews(item) {
  var strain_id = item.child("id").val();

  var obj = new Object();
  obj.info = item;

  return admin
    .database()
    .ref()
    .child("reviews")
    .child(strain_id)
    .once("value")
    .then((snapshot) => {
      if (snapshot.exists()) {
        return admin
          .database()
          .ref()
          .child("reviews")
          .child(strain_id)
          .child("data")
          .limitToLast(25)
          .once("value")
          .then((ratings) => {
            var averageRating = snapshot.child("averageRating").val();
            var numberOfReviews = snapshot.child("numberOfReviews").val();
            var ratings = ratings.val();

            obj.reviews = {
              averageRating: averageRating,
              numberOfReviews: numberOfReviews,
              ratings: ratings,
            };
            return obj;
          });
      } else {
        obj.reviews = { averageRating: 0, numberOfReviews: 0, ratings: {} };
        return obj;
      }
    });
}

function shuffle(a) {
  var j, x, i;
  for (i = a.length - 1; i > 0; i--) {
    j = Math.floor(Math.random() * (i + 1));
    x = a[i];
    a[i] = a[j];
    a[j] = x;
  }
  return a;
}
