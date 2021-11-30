const axios = require('axios');

exports.handler = async (context, event, callback) => {
    console.log('context', context);
    console.log('event', event);
    
    const toDoId = event.toDoDigits;
    const getToDos = await axios.get(`${process.env.DUMMY_API}/comments/${toDoId}`);
    console.log('getToDos', getToDos);
    
    const response = {
      todo: getToDos.data.body
    };
  
    callback(null, response);
};