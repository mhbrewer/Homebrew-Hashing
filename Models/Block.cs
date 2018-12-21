
namespace HashApp {
    public class Block {
        public int BlockId { get; set; }
        public string data { get; set; }
        public string prev { get; set; }
        public string hash { get; set; }

        public Block(int BlockId = 1, string data = "Default", string prev = "0000000000000000000000000000000000000000000000000000000000000000") {
            this.BlockId = BlockId;
            this.data = data;
            this.prev = prev;
            string combine = BlockId.ToString() + data + prev;
            this.hash = MAS256.mas256(combine);
        }
    }
}