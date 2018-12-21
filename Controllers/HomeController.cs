using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace HashApp.Controllers {
    public class HomeController : Controller {
        
        [HttpGet]
        [Route("")]
        [Route("/hash")]
        public IActionResult HashPage(string input = " ") {
            string output = MAS256.mas256(input);
            return View("HashPage", output);
        }

        [HttpGet]
        [Route("/block")]
        public IActionResult BlockPage(string input) {
            Block output;
            if(input == null) {
                output = new Block();
                return View("BlockPage", output);
            }
            output = new Block(data: input);
            return View("BlockPage", output);
        }

        [HttpGet]
        [Route("/blockchain")]
        public IActionResult BlockchainPage(string input) {
            Block[] output = new Block[3];
            if(input == null) {
                output[0] = new Block();
            } else {
                output[0] = new Block(data: input);
            }
            for(int ii = 1; ii < output.Length; ii++) {
                output[ii] = new Block(BlockId: output[ii - 1].BlockId + 1, prev: output[ii - 1].hash);
            }
            return View("BlockchainPage", output);
        }

        [HttpGet]
        [Route("/info")]
        public IActionResult Info() {
            return View();
        }
    }
}
