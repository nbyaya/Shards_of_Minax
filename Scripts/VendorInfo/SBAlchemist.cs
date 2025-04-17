using System;
using System.Collections.Generic;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Mobiles
{
    public class SBAlchemist : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo;
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBAlchemist(Mobile m)
        {
            if (m != null)
            {
                m_BuyInfo = new InternalBuyInfo(m);
            }
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo(Mobile m)
            {
                Add(new GenericBuyInfo(typeof(RefreshPotion), 15, 10, 0xF0B, 0, true));
                Add(new GenericBuyInfo(typeof(AgilityPotion), 15, 10, 0xF08, 0, true));
                Add(new GenericBuyInfo(typeof(NightSightPotion), 15, 10, 0xF06, 0, true));
                Add(new GenericBuyInfo(typeof(LesserHealPotion), 15, 10, 0xF0C, 0, true));
                Add(new GenericBuyInfo(typeof(StrengthPotion), 15, 10, 0xF09, 0, true));
                Add(new GenericBuyInfo(typeof(LesserPoisonPotion), 15, 10, 0xF0A, 0, true));
                Add(new GenericBuyInfo(typeof(LesserCurePotion), 15, 10, 0xF07, 0, true));
                Add(new GenericBuyInfo(typeof(LesserExplosionPotion), 21, 10, 0xF0D, 0, true));
                Add(new GenericBuyInfo(typeof(MortarPestle), 8, 10, 0xE9B, 0));

                Add(new GenericBuyInfo(typeof(BlackPearl), 5, 20, 0xF7A, 0));
                Add(new GenericBuyInfo(typeof(Bloodmoss), 5, 20, 0xF7B, 0));
                Add(new GenericBuyInfo(typeof(Garlic), 3, 20, 0xF84, 0));
                Add(new GenericBuyInfo(typeof(Ginseng), 3, 20, 0xF85, 0));
                Add(new GenericBuyInfo(typeof(MandrakeRoot), 3, 20, 0xF86, 0));
                Add(new GenericBuyInfo(typeof(Nightshade), 3, 20, 0xF88, 0));
                Add(new GenericBuyInfo(typeof(SpidersSilk), 3, 20, 0xF8D, 0));
                Add(new GenericBuyInfo(typeof(SulfurousAsh), 3, 20, 0xF8C, 0));
                Add(new GenericBuyInfo(typeof(SpringWater), 4, 20, 0xE24, 0));
				Add(new GenericBuyInfo(typeof(DestroyingAngel), 4, 20, 0xE24, 0));
				Add(new GenericBuyInfo(typeof(PetrafiedWood), 4, 20, 0xE24, 0));

                Add(new GenericBuyInfo(typeof(Bottle), 5, 100, 0xF0E, 0, true)); 
                Add(new GenericBuyInfo(typeof(HeatingStand), 2, 100, 0x1849, 0));
                Add(new GenericBuyInfo(typeof(SkinTingeingTincture), 1255, 20, 0xEFF, 90));
				Add(new GenericBuyInfo(typeof(TransmutationCauldron), 500, 10, 0xA5B4, 0));

                if (m.Map != Map.TerMur)
                {
                    Add(new GenericBuyInfo(typeof(HairDye), 37, 10, 0xEFF, 0));
                }
                else if (m is Zosilem)
                {
                    Add(new GenericBuyInfo(typeof(GlassblowingBook), 10637, 30, 0xFF4, 0));
                    Add(new GenericBuyInfo(typeof(SandMiningBook), 10637, 30, 0xFF4, 0));
                    Add(new GenericBuyInfo(typeof(Blowpipe), 21, 100, 0xE8A, 0x3B9));
                }
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(BlackPearl), 3); 
                Add(typeof(Bloodmoss), 3); 
                Add(typeof(MandrakeRoot), 2); 
                Add(typeof(Garlic), 2); 
                Add(typeof(Ginseng), 2); 
                Add(typeof(Nightshade), 2); 
                Add(typeof(SpidersSilk), 2); 
                Add(typeof(SulfurousAsh), 2); 
                Add(typeof(Bottle), 3);
                Add(typeof(MortarPestle), 4);
                Add(typeof(HairDye), 19);
                Add(typeof(SpringWater), 2);
				Add(typeof(DestroyingAngel), 2);
				Add(typeof(PetrafiedWood), 2);

                Add(typeof(NightSightPotion), 7);
                Add(typeof(AgilityPotion), 7);
                Add(typeof(StrengthPotion), 7);
                Add(typeof(RefreshPotion), 7);
                Add(typeof(LesserCurePotion), 7);
				Add(typeof(CurePotion), 11);
				Add(typeof(GreaterCurePotion), 15);
                Add(typeof(LesserHealPotion), 7);
				Add(typeof(HealPotion), 11);
				Add(typeof(GreaterHealPotion), 15);
				Add(typeof(LesserPoisonPotion), 7);
				Add(typeof(PoisonPotion), 9);
				Add(typeof(GreaterPoisonPotion), 13);
				Add(typeof(DeadlyPoisonPotion), 21);
				Add(typeof(LesserExplosionPotion), 10);
				Add(typeof(ExplosionPotion), 15);
				Add(typeof(GreaterExplosionPotion), 25);
                Add(typeof(Zychroline), 500);
                Add(typeof(Aetheralate), 500);
                Add(typeof(Neontrium), 500);
                Add(typeof(Oblivionate), 500);
                Add(typeof(Phantomide), 500);
                Add(typeof(Quarkothene), 500);
                Add(typeof(Stygiocarbon), 500);
                Add(typeof(Cryovitrin), 500);
                Add(typeof(Fluxidate), 500);
                Add(typeof(Novaesine), 500);
                Add(typeof(Xenocrylate), 500);
                Add(typeof(Gravitoxane), 500);
                Add(typeof(Eclipsium), 500);
                Add(typeof(Darkspirite), 500);
                Add(typeof(Photoplasmene), 500);
                Add(typeof(Vibranide), 500);
                Add(typeof(Duskenium), 500);
                Add(typeof(Chronodyne), 500);
                Add(typeof(Auroracene), 500);
                Add(typeof(Voidanate), 500);
                Add(typeof(Lumicryne), 500);
                Add(typeof(Prismalium), 500);
                Add(typeof(Etherothal), 500);
                Add(typeof(Pyrolythene), 500);
                Add(typeof(Radiacrylate), 500);
                Add(typeof(Synthionide), 500);
                Add(typeof(Morphaloxane), 500);
                Add(typeof(Astrocyne), 500);
                Add(typeof(Nyxiolate), 500);
                Add(typeof(Spectrovanate), 500);
                Add(typeof(Solvexium), 500);
                Add(typeof(Helionine), 500);
                Add(typeof(Thermodrithium), 500);
                Add(typeof(Arcvaloxate), 500);
                Add(typeof(Cinderathane), 500);
                Add(typeof(Zephyrenium), 500);
                Add(typeof(Kryotoxite), 500);
                Add(typeof(Starshardine), 500);
                Add(typeof(Omniplasium), 500);
                Add(typeof(Nebulifluxate), 500);
				
                Add(typeof(GraveNylon), 2250);
                Add(typeof(Aeroglass), 2250);
                Add(typeof(Infinityclay), 2250);
                Add(typeof(Bloodglass), 2250);
                Add(typeof(DenaturedMorphonite), 2250);
                Add(typeof(Energite), 2250);
                Add(typeof(FlammablePlasmine), 2250);
                Add(typeof(Impervanium), 2250);
                Add(typeof(NegativePhocite), 2250);
                Add(typeof(OpaqueHydragyon), 2250);
                Add(typeof(PositiveEvenium), 2250);
                Add(typeof(RefractivePotamite), 2250);
                Add(typeof(Schizonite), 2250);
                Add(typeof(Thoril), 2250);
                Add(typeof(TransparentAurarus), 2250);
                Add(typeof(Turbesium), 2250);
                Add(typeof(Uranimite), 2250);
                Add(typeof(ChargedAcoustesium), 2250);
                Add(typeof(ExoticEun), 2250);
                Add(typeof(Fibrogen), 2250);
                Add(typeof(Flurocite), 2250);
                Add(typeof(GaseousAesthogen), 2250);
                Add(typeof(HighdensityElectron), 2250);
                Add(typeof(MorphicHeteril), 2250);
                Add(typeof(Negite), 2250);
            }
        }
    }
}
