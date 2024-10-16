using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class CopperShovel : Item
    {
        
		public override string DefaultName
        {
            get { return "a copper shovel"; }
        }

        private int m_Uses;

        [Constructable]
        public CopperShovel() : base(0xF39)
        {
            Weight = 5.0;
            Hue = 0x431;
            m_Uses = 50; // Total uses
        }

        public CopperShovel(Serial serial) : base(serial)
        {
        }
		
		private static int[] m_MountainAndCaveTiles = new int[]
			{
				220, 221, 222, 223, 224, 225, 226, 227, 228, 229,
				230, 231, 236, 237, 238, 239, 240, 241, 242, 243,
				244, 245, 246, 247, 252, 253, 254, 255, 256, 257,
				258, 259, 260, 261, 262, 263, 268, 269, 270, 271,
				272, 273, 274, 275, 276, 277, 278, 279, 286, 287,
				288, 289, 290, 291, 292, 293, 294, 296, 296, 297,
				321, 322, 323, 324, 467, 468, 469, 470, 471, 472,
				473, 474, 476, 477, 478, 479, 480, 481, 482, 483,
				484, 485, 486, 487, 492, 493, 494, 495, 543, 544,
				545, 546, 547, 548, 549, 550, 551, 552, 553, 554,
				555, 556, 557, 558, 559, 560, 561, 562, 563, 564,
				565, 566, 567, 568, 569, 570, 571, 572, 573, 574,
				575, 576, 577, 578, 579, 581, 582, 583, 584, 585,
				586, 587, 588, 589, 590, 591, 592, 593, 594, 595,
				596, 597, 598, 599, 600, 601, 610, 611, 612, 613,

				1010, 1741, 1742, 1743, 1744, 1745, 1746, 1747, 1748, 1749,
				1750, 1751, 1752, 1753, 1754, 1755, 1756, 1757, 1771, 1772,
				1773, 1774, 1775, 1776, 1777, 1778, 1779, 1780, 1781, 1782,
				1783, 1784, 1785, 1786, 1787, 1788, 1789, 1790, 1801, 1802,
				1803, 1804, 1805, 1806, 1807, 1808, 1809, 1811, 1812, 1813,
				1814, 1815, 1816, 1817, 1818, 1819, 1820, 1821, 1822, 1823,
				1824, 1831, 1832, 1833, 1834, 1835, 1836, 1837, 1838, 1839,
				1840, 1841, 1842, 1843, 1844, 1845, 1846, 1847, 1848, 1849,
				1850, 1851, 1852, 1853, 1854, 1861, 1862, 1863, 1864, 1865,
				1866, 1867, 1868, 1869, 1870, 1871, 1872, 1873, 1874, 1875,
				1876, 1877, 1878, 1879, 1880, 1881, 1882, 1883, 1884, 1981,
				1982, 1983, 1984, 1985, 1986, 1987, 1988, 1989, 1990, 1991,
				1992, 1993, 1994, 1995, 1996, 1997, 1998, 1999, 2000, 2001,
				2002, 2003, 2004, 2028, 2029, 2030, 2031, 2032, 2033, 2100,
				2101, 2102, 2103, 2104, 2105,
				
				0x453B, 0x453C, 0x453D, 0x453E, 0x453F, 0x4540, 0x4541,
				0x4542, 0x4543, 0x4544,	0x4545, 0x4546, 0x4547, 0x4548,
				0x4549, 0x454A, 0x454B, 0x454C, 0x454D, 0x454E,	0x454F
			};

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Skills.Mining.Value < 150)
            {
                from.SendMessage("You need at least 150 mining skill to use this.");
                return;
            }

            if (m_Uses > 0)
            {
                from.BeginTarget(-1, true, TargetFlags.None, new TargetCallback(OnTarget));
            }
            else
            {
                from.SendMessage("This shovel has worn out.");
                this.Delete();
            }
        }

        private void OnTarget(Mobile from, object targeted)
        {
            if (targeted is StaticTarget)
            {
                StaticTarget target = (StaticTarget)targeted;

                if (Array.IndexOf(m_MountainAndCaveTiles, target.ItemID) >= 0)
                {
                    from.AddToBackpack(new CopperOre());
                    m_Uses--;

                    if (m_Uses <= 0)
                    {
                        from.SendMessage("The copper shovel breaks!");
                        this.Delete();
                    }
                }
                else
                {
                    from.AddToBackpack(new CopperOre());
                    m_Uses--;
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Uses);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Uses = reader.ReadInt();
        }
    }
}
