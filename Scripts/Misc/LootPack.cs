#region References
using System;

using Server.Items;
using Server.Mobiles;
#endregion

namespace Server
{
	public class LootPack
	{
		public static int GetLuckChance(Mobile killer, Mobile victim)
		{
			if (!Core.AOS)
			{
				return 0;
			}

			int luck = killer is PlayerMobile ? ((PlayerMobile)killer).RealLuck : killer.Luck;

            PlayerMobile pmKiller = killer as PlayerMobile;
			if (pmKiller != null && pmKiller.SentHonorContext != null && pmKiller.SentHonorContext.Target == victim)
			{
				luck += pmKiller.SentHonorContext.PerfectionLuckBonus;
			}

			if (luck < 0)
			{
				return 0;
			}

			if (!Core.SE && luck > 1200)
			{
				luck = 1200;
			}

			return GetLuckChance(luck);
		}

        public static int GetLuckChance(int luck)
        {
            return (int)(Math.Pow(luck, 1 / 1.8) * 100);
        }

		public static int GetLuckChanceForKiller(Mobile m)
		{
            BaseCreature dead = m as BaseCreature;

            if (dead == null)
                return 240;

			var list = dead.GetLootingRights();

			DamageStore highest = null;

			for (int i = 0; i < list.Count; ++i)
			{
				DamageStore ds = list[i];

				if (ds.m_HasRight && (highest == null || ds.m_Damage > highest.m_Damage))
				{
					highest = ds;
				}
			}

			if (highest == null)
			{
				return 0;
			}

			return GetLuckChance(highest.m_Mobile, dead);
		}

		public static bool CheckLuck(int chance)
		{
			return (chance > Utility.Random(10000));
		}

		private readonly LootPackEntry[] m_Entries;

		public LootPack(LootPackEntry[] entries)
		{
			m_Entries = entries;
		}

		public void Generate(Mobile from, Container cont, bool spawning, int luckChance)
		{
			if (cont == null)
			{
				return;
			}

			bool checkLuck = Core.AOS;

			for (int i = 0; i < m_Entries.Length; ++i)
			{
				LootPackEntry entry = m_Entries[i];

				bool shouldAdd = (entry.Chance > Utility.Random(10000));

				if (!shouldAdd && checkLuck)
				{
					checkLuck = false;

					if (CheckLuck(luckChance))
					{
						shouldAdd = (entry.Chance > Utility.Random(10000));
					}
				}

				if (!shouldAdd)
				{
					continue;
				}

				Item item = entry.Construct(from, luckChance, spawning);

				if (item != null)
				{
					if (!item.Stackable || !cont.TryDropItem(from, item, false))
					{
						cont.DropItem(item);
					}
				}
			}
		}

		public static readonly LootPackItem[] Gold = new[] {new LootPackItem(typeof(Gold), 1)};

		public static readonly LootPackItem[] Instruments = new[] {new LootPackItem(typeof(BaseInstrument), 1)};

		public static readonly LootPackItem[] LowScrollItems = new[]
		{
            new LootPackItem(typeof(ClumsyScroll), 1)
        };

		public static readonly LootPackItem[] MedScrollItems = new[]
		{
			new LootPackItem(typeof(ArchCureScroll), 1)
		};

		public static readonly LootPackItem[] HighScrollItems = new[]
		{
			new LootPackItem(typeof(SummonAirElementalScroll), 1)
		};

		public static readonly LootPackItem[] GemItems = new[] {new LootPackItem(typeof(Amber), 1)};

		public static readonly LootPackItem[] PotionItems = new[]
		{
			new LootPackItem(typeof(AgilityPotion), 1), new LootPackItem(typeof(StrengthPotion), 1),
			new LootPackItem(typeof(RefreshPotion), 1), new LootPackItem(typeof(LesserCurePotion), 1),
			new LootPackItem(typeof(LesserHealPotion), 1), new LootPackItem(typeof(LesserPoisonPotion), 1)
		};

		#region Old Magic Items
		public static readonly LootPackItem[] OldMagicItems = new[]
		{
			new LootPackItem(typeof(BaseJewel), 1), new LootPackItem(typeof(BaseArmor), 4),
			new LootPackItem(typeof(BaseWeapon), 3), new LootPackItem(typeof(BaseRanged), 1),
			new LootPackItem(typeof(BaseShield), 1)
		};
		#endregion

		#region AOS Magic Items
		public static readonly LootPackItem[] AosMagicItemsPoor = new[]
		{
			new LootPackItem(typeof(BaseWeapon), 3), new LootPackItem(typeof(BaseRanged), 1),
			new LootPackItem(typeof(BaseArmor), 4), new LootPackItem(typeof(BaseShield), 1),
			new LootPackItem(typeof(BaseJewel), 2)
		};

		public static readonly LootPackItem[] AosMagicItemsMeagerType1 = new[]
		{
			new LootPackItem(typeof(BaseWeapon), 56), new LootPackItem(typeof(BaseRanged), 14),
			new LootPackItem(typeof(BaseArmor), 81), new LootPackItem(typeof(BaseShield), 11),
			new LootPackItem(typeof(BaseJewel), 42)
		};

		public static readonly LootPackItem[] AosMagicItemsMeagerType2 = new[]
		{
			new LootPackItem(typeof(BaseWeapon), 28), new LootPackItem(typeof(BaseRanged), 7),
			new LootPackItem(typeof(BaseArmor), 40), new LootPackItem(typeof(BaseShield), 5),
			new LootPackItem(typeof(BaseJewel), 21)
		};

		public static readonly LootPackItem[] AosMagicItemsAverageType1 = new[]
		{
			new LootPackItem(typeof(BaseWeapon), 90), new LootPackItem(typeof(BaseRanged), 23),
			new LootPackItem(typeof(BaseArmor), 130), new LootPackItem(typeof(BaseShield), 17),
			new LootPackItem(typeof(BaseJewel), 68)
		};

		public static readonly LootPackItem[] AosMagicItemsAverageType2 = new[]
		{
			new LootPackItem(typeof(BaseWeapon), 54), new LootPackItem(typeof(BaseRanged), 13),
			new LootPackItem(typeof(BaseArmor), 77), new LootPackItem(typeof(BaseShield), 10),
			new LootPackItem(typeof(BaseJewel), 40)
		};

		public static readonly LootPackItem[] AosMagicItemsRichType1 = new[]
		{
			new LootPackItem(typeof(BaseWeapon), 211), new LootPackItem(typeof(BaseRanged), 53),
			new LootPackItem(typeof(BaseArmor), 303), new LootPackItem(typeof(BaseShield), 39),
			new LootPackItem(typeof(BaseJewel), 158)
		};

		public static readonly LootPackItem[] AosMagicItemsRichType2 = new[]
		{
			new LootPackItem(typeof(BaseWeapon), 170), new LootPackItem(typeof(BaseRanged), 43),
			new LootPackItem(typeof(BaseArmor), 245), new LootPackItem(typeof(BaseShield), 32),
			new LootPackItem(typeof(BaseJewel), 128)
		};

		public static readonly LootPackItem[] AosMagicItemsFilthyRichType1 = new[]
		{
			new LootPackItem(typeof(BaseWeapon), 219), new LootPackItem(typeof(BaseRanged), 55),
			new LootPackItem(typeof(BaseArmor), 315), new LootPackItem(typeof(BaseShield), 41),
			new LootPackItem(typeof(BaseJewel), 164)
		};

		public static readonly LootPackItem[] AosMagicItemsFilthyRichType2 = new[]
		{
			new LootPackItem(typeof(BaseWeapon), 239), new LootPackItem(typeof(BaseRanged), 60),
			new LootPackItem(typeof(BaseArmor), 343), new LootPackItem(typeof(BaseShield), 90),
			new LootPackItem(typeof(BaseJewel), 45)
		};

		public static readonly LootPackItem[] AosMagicItemsUltraRich = new[]
		{
			new LootPackItem(typeof(BaseWeapon), 276), new LootPackItem(typeof(BaseRanged), 69),
			new LootPackItem(typeof(BaseArmor), 397), new LootPackItem(typeof(BaseShield), 52),
			new LootPackItem(typeof(BaseJewel), 207)
		};
		
		public static readonly LootPackItem[] NewFilthyRichItems = new LootPackItem[]
		{
			// Make sure YourNewItem is a valid Item-derived class
			
			new LootPackItem(typeof(RandomMagicWeapon), 100), // Adjust chance as needed
			new LootPackItem(typeof(RandomMagicArmor), 100),
			new LootPackItem(typeof(RandomMagicClothing), 100),
			new LootPackItem(typeof(RandomMagicClothing), 100),
			new LootPackItem(typeof(RandomMagicClothing), 100),
			new LootPackItem(typeof(RandomMagicClothing), 100),
			new LootPackItem(typeof(RandomMagicClothing), 100),
			new LootPackItem(typeof(RandomMagicClothing), 100),
			new LootPackItem(typeof(RandomMagicJewelry), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomMagicWeapon), 100),
			new LootPackItem(typeof(RandomSkillJewelryA), 100),
			new LootPackItem(typeof(RandomSkillJewelryAA), 100),
			new LootPackItem(typeof(RandomSkillJewelryAB), 100),
			new LootPackItem(typeof(RandomSkillJewelryAC), 100),
			new LootPackItem(typeof(RandomSkillJewelryAD), 100),
			new LootPackItem(typeof(RandomSkillJewelryAE), 100),
			new LootPackItem(typeof(RandomSkillJewelryAF), 100),
			new LootPackItem(typeof(RandomSkillJewelryAG), 100),
			new LootPackItem(typeof(RandomSkillJewelryAH), 100),
			new LootPackItem(typeof(RandomSkillJewelryAI), 100),
			new LootPackItem(typeof(RandomSkillJewelryAJ), 100),
			new LootPackItem(typeof(RandomSkillJewelryAK), 100),
			new LootPackItem(typeof(RandomSkillJewelryAL), 100),
			new LootPackItem(typeof(RandomSkillJewelryAM), 100),
			new LootPackItem(typeof(RandomSkillJewelryAN), 100),
			new LootPackItem(typeof(RandomSkillJewelryAO), 100),
			new LootPackItem(typeof(RandomSkillJewelryAP), 100),
			new LootPackItem(typeof(RandomSkillJewelryB), 100),
			new LootPackItem(typeof(RandomSkillJewelryC), 100),
			new LootPackItem(typeof(RandomSkillJewelryD), 100),
			new LootPackItem(typeof(RandomSkillJewelryE), 100),
			new LootPackItem(typeof(RandomSkillJewelryF), 100),
			new LootPackItem(typeof(RandomSkillJewelryG), 100),
			new LootPackItem(typeof(RandomSkillJewelryH), 100),
			new LootPackItem(typeof(RandomSkillJewelryI), 100),
			new LootPackItem(typeof(RandomSkillJewelryJ), 100),
			new LootPackItem(typeof(RandomSkillJewelryK), 100),
			new LootPackItem(typeof(RandomSkillJewelryL), 100),
			new LootPackItem(typeof(RandomSkillJewelryM), 100),
			new LootPackItem(typeof(RandomSkillJewelryN), 100),
			new LootPackItem(typeof(RandomSkillJewelryO), 100),
			new LootPackItem(typeof(RandomSkillJewelryP), 100),
			new LootPackItem(typeof(RandomSkillJewelryQ), 100),
			new LootPackItem(typeof(RandomSkillJewelryR), 100),
			new LootPackItem(typeof(RandomSkillJewelryS), 100),
			new LootPackItem(typeof(RandomSkillJewelryT), 100),
			new LootPackItem(typeof(RandomSkillJewelryU), 100),
			new LootPackItem(typeof(RandomSkillJewelryV), 100),
			new LootPackItem(typeof(RandomSkillJewelryW), 100),
			new LootPackItem(typeof(RandomMagicJewelry), 100),
			new LootPackItem(typeof(RandomSkillJewelryY), 100),
			new LootPackItem(typeof(RandomSkillJewelryZ), 100),
			new LootPackItem(typeof(RandomMagicJewelry), 100),
			new LootPackItem(typeof(RandomMagicJewelry), 100),
			new LootPackItem(typeof(RandomMagicJewelry), 100),
			new LootPackItem(typeof(RandomMagicJewelry), 100),
			new LootPackItem(typeof(RandomMagicJewelry), 100),
			new LootPackItem(typeof(RandomMagicJewelry), 100),
			new LootPackItem(typeof(RandomMagicJewelry), 100),
			new LootPackItem(typeof(RandomMagicArmor), 100),
			new LootPackItem(typeof(RandomMagicArmor), 100),
			new LootPackItem(typeof(RandomMagicArmor), 100),
			new LootPackItem(typeof(RandomMagicArmor), 100),
			new LootPackItem(typeof(RandomMagicArmor), 100),
			new LootPackItem(typeof(RandomMagicArmor), 100),
			new LootPackItem(typeof(RandomMagicArmor), 100),
			new LootPackItem(typeof(RandomMagicArmor), 100),
			new LootPackItem(typeof(RandomMagicArmor), 100),
			new LootPackItem(typeof(RandomMagicArmor), 100), // Adjust chance as needed
			new LootPackItem(typeof(AlchemyAugmentCrystal), 100),
			new LootPackItem(typeof(AnatomyAugmentCrystal), 100),
			new LootPackItem(typeof(AnimalLoreAugmentCrystal), 100),
			new LootPackItem(typeof(AnimalTamingAugmentCrystal), 100),
			new LootPackItem(typeof(ArcheryAugmentCrystal), 100),
			new LootPackItem(typeof(ArmsLoreAugmentCrystal), 100),
			new LootPackItem(typeof(ArmSlotChangeDeed), 100),
			new LootPackItem(typeof(BagOfBombs), 100),
			new LootPackItem(typeof(BagOfHealth), 100),
			new LootPackItem(typeof(BagOfJuice), 100),
			new LootPackItem(typeof(BanishingOrb), 100),
			new LootPackItem(typeof(BanishingRod), 100),
			new LootPackItem(typeof(BeggingAugmentCrystal), 100),
			new LootPackItem(typeof(BeltSlotChangeDeed), 100),
			new LootPackItem(typeof(BlacksmithyAugmentCrystal), 100),
			new LootPackItem(typeof(BloodSword), 100),
			new LootPackItem(typeof(BootsOfCommand), 100),
			new LootPackItem(typeof(BraceletSlotChangeDeed), 100),
			new LootPackItem(typeof(BushidoAugmentCrystal), 100),
			new LootPackItem(typeof(CampingAugmentCrystal), 100),
			new LootPackItem(typeof(CapacityIncreaseDeed), 100),
			new LootPackItem(typeof(CarpentryAugmentCrystal), 100),
			new LootPackItem(typeof(CartographyAugmentCrystal), 100),
			new LootPackItem(typeof(ChestSlotChangeDeed), 100),
			new LootPackItem(typeof(ChivalryAugmentCrystal), 100),
			new LootPackItem(typeof(ColdHitAreaCrystal), 100),
			new LootPackItem(typeof(ColdResistAugmentCrystal), 100),
			new LootPackItem(typeof(CookingAugmentCrystal), 100),
			new LootPackItem(typeof(CurseAugmentCrystal), 100),
			new LootPackItem(typeof(DetectingHiddenAugmentCrystal), 100),
			new LootPackItem(typeof(DiscordanceAugmentCrystal), 100),
			new LootPackItem(typeof(DispelAugmentCrystal), 100),
			new LootPackItem(typeof(EarringSlotChangeDeed), 100),
			new LootPackItem(typeof(EnergyHitAreaCrystal), 100),
			new LootPackItem(typeof(EnergyResistAugmentCrystal), 100),
			new LootPackItem(typeof(FatigueAugmentCrystal), 100),
			new LootPackItem(typeof(FencingAugmentCrystal), 100),
			new LootPackItem(typeof(FireballAugmentCrystal), 100),
			new LootPackItem(typeof(FireHitAreaCrystal), 100),
			new LootPackItem(typeof(FireResistAugmentCrystal), 100),
			new LootPackItem(typeof(FishingAugmentCrystal), 100),
			new LootPackItem(typeof(FletchingAugmentCrystal), 100),
			new LootPackItem(typeof(FocusAugmentCrystal), 100),
			new LootPackItem(typeof(FootwearSlotChangeDeed), 100),
			new LootPackItem(typeof(ForensicEvaluationAugmentCrystal), 100),
			new LootPackItem(typeof(GlovesOfCommand), 100),
			new LootPackItem(typeof(HarmAugmentCrystal), 100),
			new LootPackItem(typeof(HeadSlotChangeDeed), 100),
			new LootPackItem(typeof(HealingAugmentCrystal), 100),
			new LootPackItem(typeof(HerdingAugmentCrystal), 100),
			new LootPackItem(typeof(HidingAugmentCrystal), 100),
			new LootPackItem(typeof(ImbuingAugmentCrystal), 100),
			new LootPackItem(typeof(InscriptionAugmentCrystal), 100),
			new LootPackItem(typeof(ItemIdentificationAugmentCrystal), 100),
			new LootPackItem(typeof(JesterHatOfCommand), 100),
			new LootPackItem(typeof(LegsSlotChangeDeed), 100),
			new LootPackItem(typeof(LifeLeechAugmentCrystal), 100),
			new LootPackItem(typeof(LightningAugmentCrystal), 100),
			new LootPackItem(typeof(LockpickingAugmentCrystal), 100),
			new LootPackItem(typeof(LowerAttackAugmentCrystal), 100),
			new LootPackItem(typeof(LuckAugmentCrystal), 100),
			new LootPackItem(typeof(LumberjackingAugmentCrystal), 100),
			new LootPackItem(typeof(MaceFightingAugmentCrystal), 100),
			new LootPackItem(typeof(MageryAugmentCrystal), 100),
			new LootPackItem(typeof(ManaDrainAugmentCrystal), 100),
			new LootPackItem(typeof(ManaLeechAugmentCrystal), 100),
			new LootPackItem(typeof(MaxxiaScroll), 100),
			new LootPackItem(typeof(MeditationAugmentCrystal), 100),
			new LootPackItem(typeof(MiningAugmentCrystal), 100),
			new LootPackItem(typeof(MirrorOfKalandra), 100),
			new LootPackItem(typeof(MusicianshipAugmentCrystal), 100),
			new LootPackItem(typeof(NeckSlotChangeDeed), 100),
			new LootPackItem(typeof(NecromancyAugmentCrystal), 100),
			new LootPackItem(typeof(NinjitsuAugmentCrystal), 100),
			new LootPackItem(typeof(OneHandedTransformDeed), 100),
			new LootPackItem(typeof(ParryingAugmentCrystal), 100),
			new LootPackItem(typeof(PeacemakingAugmentCrystal), 100),
			new LootPackItem(typeof(PhysicalHitAreaCrystal), 100),
			new LootPackItem(typeof(PhysicalResistAugmentCrystal), 100),
			new LootPackItem(typeof(PlateLeggingsOfCommand), 100),
			new LootPackItem(typeof(PoisonHitAreaCrystal), 100),
			new LootPackItem(typeof(PoisoningAugmentCrystal), 100),
			new LootPackItem(typeof(PoisonResistAugmentCrystal), 100),
			new LootPackItem(typeof(ProvocationAugmentCrystal), 100),
			new LootPackItem(typeof(RemoveTrapAugmentCrystal), 100),
			new LootPackItem(typeof(ResistingSpellsAugmentCrystal), 100),
			new LootPackItem(typeof(RingSlotChangeDeed), 100),
			new LootPackItem(typeof(RodOfOrcControl), 100),
			new LootPackItem(typeof(ShirtSlotChangeDeed), 100),
			new LootPackItem(typeof(SnoopingAugmentCrystal), 100),
			new LootPackItem(typeof(SpellweavingAugmentCrystal), 100),
			new LootPackItem(typeof(SpiritSpeakAugmentCrystal), 100),
			new LootPackItem(typeof(StaminaLeechAugmentCrystal), 100),
			new LootPackItem(typeof(StealingAugmentCrystal), 100),
			new LootPackItem(typeof(StealthAugmentCrystal), 100),
			new LootPackItem(typeof(SwingSpeedAugmentCrystal), 100),
			new LootPackItem(typeof(SwordsmanshipAugmentCrystal), 100),
			new LootPackItem(typeof(TacticsAugmentCrystal), 100),
			new LootPackItem(typeof(TailoringAugmentCrystal), 100),
			new LootPackItem(typeof(TalismanSlotChangeDeed), 100),
			new LootPackItem(typeof(TasteIDAugmentCrystal), 100),
			new LootPackItem(typeof(ThrowingAugmentCrystal), 100),
			new LootPackItem(typeof(TinkeringAugmentCrystal), 100),
			new LootPackItem(typeof(TrackingAugmentCrystal), 100),
			new LootPackItem(typeof(VeterinaryAugmentCrystal), 100),
			new LootPackItem(typeof(WeaponSpeedAugmentCrystal), 100),
			new LootPackItem(typeof(WrestlingAugmentCrystal), 100),
			new LootPackItem(typeof(PetSlotDeed), 100),
			new LootPackItem(typeof(PetBondDeed), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(StatCapOrb), 100),
			new LootPackItem(typeof(SkillOrb), 100),
			new LootPackItem(typeof(AbysmalHorrorSummoningMateria), 100),
			new LootPackItem(typeof(AcidElementalSummoningMateria), 100),
			new LootPackItem(typeof(AgapiteElementalSummoningMateria), 100),
			new LootPackItem(typeof(AirElementalSummoningMateria), 100),
			new LootPackItem(typeof(AlligatorSummoningMateria), 100),
			new LootPackItem(typeof(AncientLichSummoningMateria), 100),
			new LootPackItem(typeof(AncientWyrmSummoningMateria), 100),
			new LootPackItem(typeof(AntLionSummoningMateria), 100),
			new LootPackItem(typeof(ArcaneDaemonSummoningMateria), 100),
			new LootPackItem(typeof(ArcticOgreLordSummoningMateria), 100),
			new LootPackItem(typeof(AxeBreathMateria), 100),
			new LootPackItem(typeof(AxeCircleMateria), 100),
			new LootPackItem(typeof(AxeLineMateria), 100),
			new LootPackItem(typeof(BakeKitsuneSummoningMateria), 100),
			new LootPackItem(typeof(BalronSummoningMateria), 100),
			new LootPackItem(typeof(BarracoonSummoningMateria), 100),
			new LootPackItem(typeof(BeeBreathMateria), 100),
			new LootPackItem(typeof(BeeCircleMateria), 100),
			new LootPackItem(typeof(BeeLineMateria), 100),
			new LootPackItem(typeof(BeetleSummoningMateria), 100),
			new LootPackItem(typeof(BlackBearSummoningMateria), 100),
			new LootPackItem(typeof(BlackDragoonPirateMateria), 100),
			new LootPackItem(typeof(BlackSolenInfiltratorQueenSummoningMateria), 100),
			new LootPackItem(typeof(BlackSolenInfiltratorWarriorMateria), 100),
			new LootPackItem(typeof(BlackSolenQueenSummoningMateria), 100),
			new LootPackItem(typeof(BlackSolenWarriorSummoningMateria), 100),
			new LootPackItem(typeof(BlackSolenWorkerSummoningMateria), 100),
			new LootPackItem(typeof(BladesBreathMateria), 100),
			new LootPackItem(typeof(BladesCircleMateria), 100),
			new LootPackItem(typeof(BladesLineMateria), 100),
			new LootPackItem(typeof(BloodElementalSummoningGem), 100),
			new LootPackItem(typeof(BloodSwarmGem), 100),
			new LootPackItem(typeof(BoarSummoningMateria), 100),
			new LootPackItem(typeof(BogleSummoningMateria), 100),
			new LootPackItem(typeof(BoglingSummoningMateria), 100),
			new LootPackItem(typeof(BogThingSummoningMateria), 100),
			new LootPackItem(typeof(BoneDemonSummoningMateria), 100),
			new LootPackItem(typeof(BoneKnightSummoningMateria), 100),
			new LootPackItem(typeof(BoneMagiSummoningMateria), 100),
			new LootPackItem(typeof(BoulderBreathMateria), 100),
			new LootPackItem(typeof(BoulderCircleMateria), 100),
			new LootPackItem(typeof(BoulderLineMateria), 100),
			new LootPackItem(typeof(BrigandSummoningMateria), 100),
			new LootPackItem(typeof(BronzeElementalSummoningMateria), 100),
			new LootPackItem(typeof(BrownBearSummoningMateria), 100),
			new LootPackItem(typeof(BullFrogSummoningMateria), 100),
			new LootPackItem(typeof(BullSummoningMateria), 100),
			new LootPackItem(typeof(CatSummoningMateria), 100),
			new LootPackItem(typeof(CentaurSummoningMateria), 100),
			new LootPackItem(typeof(ChaosDaemonSummoningMateria), 100),
			new LootPackItem(typeof(ChaosDragoonEliteSummoningMateria), 100),
			new LootPackItem(typeof(ChaosDragoonSummoningMateria), 100),
			new LootPackItem(typeof(ChickenSummoningMateria), 100),
			new LootPackItem(typeof(CopperElementalSummoningMateria), 100),
			new LootPackItem(typeof(CorpserSummoningMateria), 100),
			new LootPackItem(typeof(CorrosiveSlimeSummoningMateria), 100),
			new LootPackItem(typeof(CorruptedSoulMateria), 100),
			new LootPackItem(typeof(CougarSummoningMateria), 100),
			new LootPackItem(typeof(CowSummoningMateria), 100),
			new LootPackItem(typeof(CraneSummoningMateria), 100),
			new LootPackItem(typeof(CrankBreathMateria), 100),
			new LootPackItem(typeof(CrankCircleMateria), 100),
			new LootPackItem(typeof(CrankLineMateria), 100),
			new LootPackItem(typeof(CrimsonDragonSummoningMateria), 100),
			new LootPackItem(typeof(CrystalElementalSummoningMateria), 100),
			new LootPackItem(typeof(CurtainBreathMateria), 100),
			new LootPackItem(typeof(CurtainCircleMateria), 100),
			new LootPackItem(typeof(CurtainLineMateria), 100),
			new LootPackItem(typeof(CuSidheSummoningMateria), 100),
			new LootPackItem(typeof(CyclopsSummoningMateria), 100),
			new LootPackItem(typeof(DaemonSummoningMateria), 100),
			new LootPackItem(typeof(DarkWispSummoningMateria), 100),
			new LootPackItem(typeof(DarkWolfSummoningMateria), 100),
			new LootPackItem(typeof(DeathWatchBeetleSummoningMateria), 100),
			new LootPackItem(typeof(DeepSeaSerpentSummoningMateria), 100),
			new LootPackItem(typeof(DeerBreathMateria), 100),
			new LootPackItem(typeof(DeerCircleMateria), 100),
			new LootPackItem(typeof(DeerLineMateria), 100),
			new LootPackItem(typeof(DemonKnightSummoningMateria), 100),
			new LootPackItem(typeof(DesertOstardSummoningMateria), 100),
			new LootPackItem(typeof(DevourerSummoningMateria), 100),
			new LootPackItem(typeof(DireWolfSummoningMateria), 100),
			new LootPackItem(typeof(DogSummoningMateria), 100),
			new LootPackItem(typeof(DolphinSummoningMateria), 100),
			new LootPackItem(typeof(DopplegangerSummoningMateria), 100),
			new LootPackItem(typeof(DragonSummoningMateria), 100),
			new LootPackItem(typeof(DrakeSummoningMateria), 100),
			new LootPackItem(typeof(DreadSpiderSummoningMateria), 100),
			new LootPackItem(typeof(DullCopperElementalSummoningMateria), 100),
			new LootPackItem(typeof(DVortexBreathMateria), 100),
			new LootPackItem(typeof(DVortexCircleMateria), 100),
			new LootPackItem(typeof(DVortexLineMateria), 100),
			new LootPackItem(typeof(EagleSummoningMateria), 100),
			new LootPackItem(typeof(EarthElementalSummoningMateria), 100),
			new LootPackItem(typeof(EfreetSummoningMateria), 100),
			new LootPackItem(typeof(ElderGazerSummoningMateria), 100),
			new LootPackItem(typeof(EliteNinjaSummoningMateria), 100),
			new LootPackItem(typeof(EttinSummoningMateria), 100),
			new LootPackItem(typeof(EvilHealerSummoningMateria), 100),
			new LootPackItem(typeof(EvilMageSummoningMateria), 100),
			new LootPackItem(typeof(ExecutionerMateria), 100),
			new LootPackItem(typeof(ExodusMinionSummoningMateria), 100),
			new LootPackItem(typeof(ExodusOverseerSummoningMateria), 100),
			new LootPackItem(typeof(FanDancerSummoningMateria), 100),
			new LootPackItem(typeof(FeralTreefellowSummoningMateria), 100),
			new LootPackItem(typeof(FetidEssenceMateria), 100),
			new LootPackItem(typeof(FireBeetleSummoningMateria), 100),
			new LootPackItem(typeof(FireElementalSummoningMateria), 100),
			new LootPackItem(typeof(FireGargoyleSummoningMateria), 100),
			new LootPackItem(typeof(FireSteedSummoningMateria), 100),
			new LootPackItem(typeof(FlaskBreathMateria), 100),
			new LootPackItem(typeof(FlaskCircleMateria), 100),
			new LootPackItem(typeof(FlaskLineMateria), 100),
			new LootPackItem(typeof(FleshGolemSummoningMateria), 100),
			new LootPackItem(typeof(FleshRendererSummoningMateria), 100),
			new LootPackItem(typeof(ForestOstardSummoningMateria), 100),
			new LootPackItem(typeof(FrenziedOstardSummoningMateria), 100),
			new LootPackItem(typeof(FrostOozeSummoningMateria), 100),
			new LootPackItem(typeof(FrostSpiderSummoningMateria), 100),
			new LootPackItem(typeof(FrostTrollSummoningMateria), 100),
			new LootPackItem(typeof(FTreeCircleMateria), 100),
			new LootPackItem(typeof(FTreeLineMateria), 100),
			new LootPackItem(typeof(GamanSummoningMateria), 100),
			new LootPackItem(typeof(GargoyleSummoningMateria), 100),
			new LootPackItem(typeof(GasBreathMateria), 100),
			new LootPackItem(typeof(GasCircleMateria), 100),
			new LootPackItem(typeof(GasLineMateria), 100),
			new LootPackItem(typeof(GateBreathMateria), 100),
			new LootPackItem(typeof(GateCircleMateria), 100),
			new LootPackItem(typeof(GateLineMateria), 100),
			new LootPackItem(typeof(GazerSummoningMateria), 100),
			new LootPackItem(typeof(GhoulSummoningMateria), 100),
			new LootPackItem(typeof(GiantBlackWidowSummoningMateria), 100),
			new LootPackItem(typeof(GiantRatSummoningMateria), 100),
			new LootPackItem(typeof(GiantSerpentSummoningMateria), 100),
			new LootPackItem(typeof(GiantSpiderSummoningMateria), 100),
			new LootPackItem(typeof(GiantToadSummoningMateria), 100),
			new LootPackItem(typeof(GibberlingSummoningMateria), 100),
			new LootPackItem(typeof(GlowBreathMateria), 100),
			new LootPackItem(typeof(GlowCircleMateria), 100),
			new LootPackItem(typeof(GlowLineMateria), 100),
			new LootPackItem(typeof(GoatSummoningMateria), 100),
			new LootPackItem(typeof(GoldenElementalSummoningMateria), 100),
			new LootPackItem(typeof(GolemSummoningMateria), 100),
			new LootPackItem(typeof(GoreFiendSummoningMateria), 100),
			new LootPackItem(typeof(GorillaSummoningMateria), 100),
			new LootPackItem(typeof(GreaterDragonSummoningMateria), 100),
			new LootPackItem(typeof(GreaterMongbatSummoningMateria), 100),
			new LootPackItem(typeof(GreatHartSummoningMateria), 100),
			new LootPackItem(typeof(GreyWolfSummoningMateria), 100),
			new LootPackItem(typeof(GrizzlyBearSummoningMateria), 100),
			new LootPackItem(typeof(GuillotineBreathMateria), 100),
			new LootPackItem(typeof(GuillotineCircleMateria), 100),
			new LootPackItem(typeof(GuillotineLineMateria), 100),
			new LootPackItem(typeof(HarpySummoningMateria), 100),
			new LootPackItem(typeof(HeadBreathMateria), 100),
			new LootPackItem(typeof(HeadCircleMateria), 100),
			new LootPackItem(typeof(HeadlessOneSummoningMateria), 100),
			new LootPackItem(typeof(HeadLineMateria), 100),
			new LootPackItem(typeof(HealerMateria), 100),
			new LootPackItem(typeof(HeartBreathMateria), 100),
			new LootPackItem(typeof(HeartCircleMateria), 100),
			new LootPackItem(typeof(HeartLineMateria), 100),
			new LootPackItem(typeof(HellCatSummoningMateria), 100),
			new LootPackItem(typeof(HellHoundSummoningMateria), 100),
			new LootPackItem(typeof(HellSteedSummoningMateria), 100),
			new LootPackItem(typeof(HindSummoningMateria), 100),
			new LootPackItem(typeof(HiryuSummoningMateria), 100),
			new LootPackItem(typeof(HorseSummoningMateria), 100),
			new LootPackItem(typeof(IceElementalSummoningMateria), 100),
			new LootPackItem(typeof(IceFiendSummoningMateria), 100),
			new LootPackItem(typeof(IceSerpentSummoningMateria), 100),
			new LootPackItem(typeof(IceSnakeSummoningMateria), 100),
			new LootPackItem(typeof(ImpSummoningMateria), 100),
			new LootPackItem(typeof(JackRabbitSummoningMateria), 100),
			new LootPackItem(typeof(KazeKemonoSummoningMateria), 100),
			new LootPackItem(typeof(KirinSummoningMateria), 100),
			new LootPackItem(typeof(LavaLizardSummoningMateria), 100),
			new LootPackItem(typeof(LavaSerpentSummoningMateria), 100),
			new LootPackItem(typeof(LavaSnakeSummoningMateria), 100),
			new LootPackItem(typeof(LesserHiryuSummoningMateria), 100),
			new LootPackItem(typeof(LichLordSummoningMateria), 100),
			new LootPackItem(typeof(LichSummoningMateria), 100),
			new LootPackItem(typeof(LizardmanSummoningMateria), 100),
			new LootPackItem(typeof(LlamaSummoningMateria), 100),
			new LootPackItem(typeof(MaidenBreathMateria), 100),
			new LootPackItem(typeof(MaidenCircleMateria), 100),
			new LootPackItem(typeof(MaidenLineMateria), 100),
			new LootPackItem(typeof(MinotaurCaptainSummoningMateria), 100),
			new LootPackItem(typeof(MountainGoatSummoningMateria), 100),
			new LootPackItem(typeof(MummySummoningMateria), 100),
			new LootPackItem(typeof(MushroomBreathMateria), 100),
			new LootPackItem(typeof(MushroomCircleMateria), 100),
			new LootPackItem(typeof(MushroomLineMateria), 100),
			new LootPackItem(typeof(NightmareSummoningMateria), 100),
			new LootPackItem(typeof(NutcrackerBreathMateria), 100),
			new LootPackItem(typeof(NutcrackerCircleMateria), 100),
			new LootPackItem(typeof(NutcrackerLineMateria), 100),
			new LootPackItem(typeof(OFlaskBreathMateria), 100),
			new LootPackItem(typeof(OFlaskCircleMateria), 100),
			new LootPackItem(typeof(OFlaskMateria), 100),
			new LootPackItem(typeof(OgreLordSummoningMateria), 100),
			new LootPackItem(typeof(OgreSummoningMateria), 100),
			new LootPackItem(typeof(OniSummoningMateria), 100),
			new LootPackItem(typeof(OphidianArchmageSummoningMateria), 100),
			new LootPackItem(typeof(OphidianKnightSummoningMateria), 100),
			new LootPackItem(typeof(OrcBomberSummoningMateria), 100),
			new LootPackItem(typeof(OrcBruteSummoningMateria), 100),
			new LootPackItem(typeof(OrcCaptainSummoningMateria), 100),
			new LootPackItem(typeof(OrcishLordSummoningMateria), 100),
			new LootPackItem(typeof(OrcishMageSummoningMateria), 100),
			new LootPackItem(typeof(OrcSummoningMateria), 100),
			new LootPackItem(typeof(PackHorseSummoningMateria), 100),
			new LootPackItem(typeof(PackLlamaSummoningMateria), 100),
			new LootPackItem(typeof(PantherSummoningMateria), 100),
			new LootPackItem(typeof(ParaBreathMateria), 100),
			new LootPackItem(typeof(ParaCircleMateria), 100),
			new LootPackItem(typeof(ParaLineMateria), 100),
			new LootPackItem(typeof(PhoenixSummoningMateria), 100),
			new LootPackItem(typeof(PigSummoningMateria), 100),
			new LootPackItem(typeof(PixieSummoningMateria), 100),
			new LootPackItem(typeof(PlagueBeastSummoningMateria), 100),
			new LootPackItem(typeof(PoisonElementalSummoningMateria), 100),
			new LootPackItem(typeof(PolarBearSummoningMateria), 100),
			new LootPackItem(typeof(RabbitSummoningMateria), 100),
			new LootPackItem(typeof(RaiJuSummoningMateria), 100),
			new LootPackItem(typeof(RatmanArcherSummoningMateria), 100),
			new LootPackItem(typeof(RatmanMageSummoningMateria), 100),
			new LootPackItem(typeof(RatmanSummoningMateria), 100),
			new LootPackItem(typeof(RatSummoningMateria), 100),
			new LootPackItem(typeof(ReaperSummoningMateria), 100),
			new LootPackItem(typeof(RevenantSummoningMateria), 100),
			new LootPackItem(typeof(RidgebackSummoningMateria), 100),
			new LootPackItem(typeof(RikktorSummoningMateria), 100),
			new LootPackItem(typeof(RoninSummoningMateria), 100),
			new LootPackItem(typeof(RuneBeetleSummoningMateria), 100),
			new LootPackItem(typeof(RuneBreathMateria), 100),
			new LootPackItem(typeof(RuneCircleMateria), 100),
			new LootPackItem(typeof(RuneLineMateria), 100),
			new LootPackItem(typeof(SatyrSummoningMateria), 100),
			new LootPackItem(typeof(SavageShamanSummoningMateria), 100),
			new LootPackItem(typeof(SavageSummoningMateria), 100),
			new LootPackItem(typeof(SawBreathMateria), 100),
			new LootPackItem(typeof(SawCircleMateria), 100),
			new LootPackItem(typeof(SawLineMateria), 100),
			new LootPackItem(typeof(ScaledSwampDragonSummoningMateria), 100),
			new LootPackItem(typeof(ScorpionSummoningMateria), 100),
			new LootPackItem(typeof(SeaSerpentSummoningMateria), 100),
			new LootPackItem(typeof(ShadowWispSummoningMateria), 100),
			new LootPackItem(typeof(ShadowWyrmSummoningMateria), 100),
			new LootPackItem(typeof(SheepSummoningMateria), 100),
			new LootPackItem(typeof(SilverSerpentSummoningMateria), 100),
			new LootPackItem(typeof(SilverSteedSummoningMateria), 100),
			new LootPackItem(typeof(SkeletalDragonSummoningMateria), 100),
			new LootPackItem(typeof(SkeletalKnightSummoningMateria), 100),
			new LootPackItem(typeof(SkeletalMageSummoningMateria), 100),
			new LootPackItem(typeof(SkeletalMountSummoningMateria), 100),
			new LootPackItem(typeof(SkeletonBreathMateria), 100),
			new LootPackItem(typeof(SkeletonCircleMateria), 100),
			new LootPackItem(typeof(SkeletonLineMateria), 100),
			new LootPackItem(typeof(SkeletonSummoningMateria), 100),
			new LootPackItem(typeof(SkullBreathMateria), 100),
			new LootPackItem(typeof(SkullCircleMateria), 100),
			new LootPackItem(typeof(SkullLineMateria), 100),
			new LootPackItem(typeof(SlimeSummoningMateria), 100),
			new LootPackItem(typeof(SmokeBreathMateria), 100),
			new LootPackItem(typeof(SmokeCircleMateria), 100),
			new LootPackItem(typeof(SmokeLineMateria), 100),
			new LootPackItem(typeof(SnakeSummoningMateria), 100),
			new LootPackItem(typeof(SnowElementalSummoningMateria), 100),
			new LootPackItem(typeof(SnowLeopardSummoningMateria), 100),
			new LootPackItem(typeof(SocketDeed), 100),
			new LootPackItem(typeof(SocketDeed1), 100),
			new LootPackItem(typeof(SocketDeed2), 100),
			new LootPackItem(typeof(SocketDeed3), 100),
			new LootPackItem(typeof(SocketDeed4), 100),
			new LootPackItem(typeof(SocketDeed5), 100),
			new LootPackItem(typeof(SparkleBreathMateria), 100),
			new LootPackItem(typeof(SparkleCircleMateria), 100),
			new LootPackItem(typeof(SparkleLineMateria), 100),
			new LootPackItem(typeof(SpikeBreathMateria), 100),
			new LootPackItem(typeof(SpikeCircleMateria), 100),
			new LootPackItem(typeof(SpikeLineMateria), 100),
			new LootPackItem(typeof(StoneBreathMateria), 100),
			new LootPackItem(typeof(StoneCircleMateria), 100),
			new LootPackItem(typeof(StoneLineMateria), 100),
			new LootPackItem(typeof(SuccubusSummoningMateria), 100),
			new LootPackItem(typeof(TimeBreathMateria), 100),
			new LootPackItem(typeof(TimeCircleMateria), 100),
			new LootPackItem(typeof(TimeLineMateria), 100),
			new LootPackItem(typeof(TitanSummoningMateria), 100),
			new LootPackItem(typeof(ToxicElementalSummoningMateria), 100),
			new LootPackItem(typeof(TrapBreathMateria), 100),
			new LootPackItem(typeof(TrapCircleMateria), 100),
			new LootPackItem(typeof(TrapLineMateria), 100),
			new LootPackItem(typeof(TreeBreathMateria), 100),
			new LootPackItem(typeof(TroglodyteSummoningMateria), 100),
			new LootPackItem(typeof(TrollSummoningMateria), 100),
			new LootPackItem(typeof(UnicornSummoningMateria), 100),
			new LootPackItem(typeof(ValoriteElementalSummoningMateria), 100),
			new LootPackItem(typeof(VampireBatSummoningMateria), 100),
			new LootPackItem(typeof(VeriteElementalSummoningMateria), 100),
			new LootPackItem(typeof(VortexBreathMateria), 100),
			new LootPackItem(typeof(VortexCircleMateria), 100),
			new LootPackItem(typeof(VortexLineMateria), 100),
			new LootPackItem(typeof(WalrusSummoningMateria), 100),
			new LootPackItem(typeof(WaterBreathMateria), 100),
			new LootPackItem(typeof(WaterCircleMateria), 100),
			new LootPackItem(typeof(WaterElementalSummoningMateria), 100),
			new LootPackItem(typeof(WaterLineMateria), 100),
			new LootPackItem(typeof(WhiteWolfSummoningMateria), 100),
			new LootPackItem(typeof(WhiteWyrmSummoningMateria), 100),
			new LootPackItem(typeof(WispSummoningMateria), 100),
			new LootPackItem(typeof(WraithSummoningMateria), 100),
			new LootPackItem(typeof(WyvernSummoningMateria), 100),
			new LootPackItem(typeof(ZombieSummoningMateria), 100),
			new LootPackItem(typeof(MythicAmethyst), 100),
			new LootPackItem(typeof(LegendaryAmethyst), 100),
			new LootPackItem(typeof(AncientAmethyst), 100),
			new LootPackItem(typeof(FenCrystal), 100),
			new LootPackItem(typeof(RhoCrystal), 100),
			new LootPackItem(typeof(RysCrystal), 100),
			new LootPackItem(typeof(WyrCrystal), 100),
			new LootPackItem(typeof(FreCrystal), 100),
			new LootPackItem(typeof(TorCrystal), 100),
			new LootPackItem(typeof(VelCrystal), 100),
			new LootPackItem(typeof(XenCrystal), 100),
			new LootPackItem(typeof(PolCrystal), 100),
			new LootPackItem(typeof(WolCrystal), 100),
			new LootPackItem(typeof(BalCrystal), 100),
			new LootPackItem(typeof(TalCrystal), 100),
			new LootPackItem(typeof(JalCrystal), 100),
			new LootPackItem(typeof(RalCrystal), 100),
			new LootPackItem(typeof(KalCrystal), 100),
			new LootPackItem(typeof(MythicDiamond), 100),
			new LootPackItem(typeof(LegendaryDiamond), 100),
			new LootPackItem(typeof(AncientDiamond), 100),
			new LootPackItem(typeof(MythicEmerald), 100),
			new LootPackItem(typeof(LegendaryEmerald), 100),
			new LootPackItem(typeof(AncientEmerald), 100),
			new LootPackItem(typeof(KeyAugment), 100),
			new LootPackItem(typeof(RadiantRhoCrystal), 100),
			new LootPackItem(typeof(RadiantRysCrystal), 100),
			new LootPackItem(typeof(RadiantWyrCrystal), 100),
			new LootPackItem(typeof(RadiantFreCrystal), 100),
			new LootPackItem(typeof(RadiantTorCrystal), 100),
			new LootPackItem(typeof(RadiantVelCrystal), 100),
			new LootPackItem(typeof(RadiantXenCrystal), 100),
			new LootPackItem(typeof(RadiantPolCrystal), 100),
			new LootPackItem(typeof(RadiantWolCrystal), 100),
			new LootPackItem(typeof(RadiantBalCrystal), 100),
			new LootPackItem(typeof(RadiantTalCrystal), 100),
			new LootPackItem(typeof(RadiantJalCrystal), 100),
			new LootPackItem(typeof(RadiantRalCrystal), 100),
			new LootPackItem(typeof(RadiantKalCrystal), 100),
			new LootPackItem(typeof(MythicRuby), 100),
			new LootPackItem(typeof(LegendaryRuby), 100),
			new LootPackItem(typeof(AncientRuby), 100),
			new LootPackItem(typeof(TyrRune), 100),
			new LootPackItem(typeof(AhmRune), 100),
			new LootPackItem(typeof(MorRune), 100),
			new LootPackItem(typeof(MefRune), 100),
			new LootPackItem(typeof(YlmRune), 100),
			new LootPackItem(typeof(KotRune), 100),
			new LootPackItem(typeof(JorRune), 100),
			new LootPackItem(typeof(MythicSapphire), 100),
			new LootPackItem(typeof(LegendarySapphire), 100),
			new LootPackItem(typeof(AncientSapphire), 100),
			new LootPackItem(typeof(MythicSkull), 100),
			new LootPackItem(typeof(AncientSkull), 100),
			new LootPackItem(typeof(LegendarySkull), 100),
			new LootPackItem(typeof(GlimmeringGranite), 100),
			new LootPackItem(typeof(GlimmeringClay), 100),
			new LootPackItem(typeof(GlimmeringHeartstone), 100),
			new LootPackItem(typeof(GlimmeringGypsum), 100),
			new LootPackItem(typeof(GlimmeringIronOre), 100),
			new LootPackItem(typeof(GlimmeringOnyx), 100),
			new LootPackItem(typeof(GlimmeringMarble), 100),
			new LootPackItem(typeof(GlimmeringPetrifiedWood), 100),
			new LootPackItem(typeof(GlimmeringLimestone), 100),
			new LootPackItem(typeof(GlimmeringBloodrock), 100),
			new LootPackItem(typeof(MythicTourmaline), 100),
			new LootPackItem(typeof(LegendaryTourmaline), 100),
			new LootPackItem(typeof(AncientTourmaline), 100),
			new LootPackItem(typeof(MythicWood), 100),
			new LootPackItem(typeof(LegendaryWood), 100),
			new LootPackItem(typeof(BootsOfCommand), 25),
			new LootPackItem(typeof(GlovesOfCommand), 25),
			new LootPackItem(typeof(GrandmastersRobe), 25),
			new LootPackItem(typeof(JesterHatOfCommand), 25),
			new LootPackItem(typeof(PlateLeggingsOfCommand), 25),
			new LootPackItem(typeof(AshAxe), 25),
			new LootPackItem(typeof(BraceletOfNaturesBounty), 25),
			new LootPackItem(typeof(CampersBackpack), 25),
			new LootPackItem(typeof(ExtraPack), 25),
			new LootPackItem(typeof(FrostwoodAxe), 25),
			new LootPackItem(typeof(GoldenCrown), 25),
			new LootPackItem(typeof(GoldenDragon), 25),
			new LootPackItem(typeof(GoldenDrakelingScaleShield), 25),
			new LootPackItem(typeof(HeartwoodAxe), 25),
			new LootPackItem(typeof(IcicleStaff), 25),
			new LootPackItem(typeof(LightLordsScepter), 25),
			new LootPackItem(typeof(MasterBall), 25),
			new LootPackItem(typeof(MasterWeaponOil), 25),
			new LootPackItem(typeof(Pokeball), 25),
			new LootPackItem(typeof(ShadowIronShovel), 25),
			new LootPackItem(typeof(StolenTile), 25),
			new LootPackItem(typeof(TrapGloves), 25),
			new LootPackItem(typeof(TrapGorget), 25),
			new LootPackItem(typeof(TrapLegs), 25),
			new LootPackItem(typeof(TrapSleeves), 25),
			new LootPackItem(typeof(TrapTunic), 25),
			new LootPackItem(typeof(WeaponOil), 25),
			new LootPackItem(typeof(WizardKey), 25),
			new LootPackItem(typeof(YewAxe), 25),
			new LootPackItem(typeof(AssassinsDagger), 25),
			new LootPackItem(typeof(BagOfBombs), 25),
			new LootPackItem(typeof(BagOfHealth), 25),
			new LootPackItem(typeof(BagOfJuice), 25),
			new LootPackItem(typeof(BanishingOrb), 25),
			new LootPackItem(typeof(BanishingRod), 25),
			new LootPackItem(typeof(BeggarKingsCrown), 25),
			new LootPackItem(typeof(BloodSword), 25),
			new LootPackItem(typeof(BloodwoodAxe), 25),
			new LootPackItem(typeof(GlovesOfTheGrandmasterThief), 25),
			new LootPackItem(typeof(MagicMasterKey), 25),
			new LootPackItem(typeof(PlantingGloves), 25),
			new LootPackItem(typeof(QuickswordEnilno), 25),
			new LootPackItem(typeof(RodOfOrcControl), 25),
			new LootPackItem(typeof(ScryingOrb), 25),
			new LootPackItem(typeof(SiegeHammer), 25),
			new LootPackItem(typeof(SnoopersMasterScope), 25),
			new LootPackItem(typeof(ThiefsGlove), 25),
			new LootPackItem(typeof(TileExcavatorShovel), 25),
			new LootPackItem(typeof(TomeOfTime), 25),
			new LootPackItem(typeof(UniversalAbsorbingDyeTub), 25),
			new LootPackItem(typeof(AegisOfAthena), 25),
			new LootPackItem(typeof(AegisOfValor), 25),
			new LootPackItem(typeof(AlchemistsAmbition), 25),
			new LootPackItem(typeof(AlchemistsConduit), 25),
			new LootPackItem(typeof(AlchemistsGroundedBoots), 25),
			new LootPackItem(typeof(AlchemistsHeart), 25),
			new LootPackItem(typeof(AlchemistsPreciseGloves), 25),
			new LootPackItem(typeof(AlchemistsResilientLeggings), 25),
			new LootPackItem(typeof(AlchemistsVisionaryHelm), 25),
			new LootPackItem(typeof(ApronOfFlames), 25),
			new LootPackItem(typeof(ArkainesValorArms), 25),
			new LootPackItem(typeof(ArtisansCraftedGauntlets), 25),
			new LootPackItem(typeof(ArtisansHelm), 25),
			new LootPackItem(typeof(AshlandersResilience), 25),
			new LootPackItem(typeof(AstartesBattlePlate), 25),
			new LootPackItem(typeof(AstartesGauntletsOfMight), 25),
			new LootPackItem(typeof(AstartesHelmOfVigilance), 25),
			new LootPackItem(typeof(AstartesShoulderGuard), 25),
			new LootPackItem(typeof(AstartesWarBoots), 25),
			new LootPackItem(typeof(AstartesWarGreaves), 25),
			new LootPackItem(typeof(AtzirisStep), 25),
			new LootPackItem(typeof(AVALANCHEDefender), 25),
			new LootPackItem(typeof(AvatarsVestments), 25),
			new LootPackItem(typeof(BardsNimbleStep), 25),
			new LootPackItem(typeof(BeastmastersCrown), 25),
			new LootPackItem(typeof(BeastmastersGrips), 25),
			new LootPackItem(typeof(BeastsWhisperersRobe), 25),
			new LootPackItem(typeof(BerserkersEmbrace), 25),
			new LootPackItem(typeof(BlackMagesMysticRobe), 25),
			new LootPackItem(typeof(BlackMagesRuneRobe), 25),
			new LootPackItem(typeof(BlacksmithsBurden), 25),
			new LootPackItem(typeof(BlackthornesSpur), 25),
			new LootPackItem(typeof(BladedancersCloseHelm), 25),
			new LootPackItem(typeof(BladedancersOrderShield), 25),
			new LootPackItem(typeof(BladedancersPlateArms), 25),
			new LootPackItem(typeof(BladeDancersPlateChest), 25),
			new LootPackItem(typeof(BladeDancersPlateLegs), 25),
			new LootPackItem(typeof(BlazePlateLegs), 25),
			new LootPackItem(typeof(BombDisposalPlate), 25),
			new LootPackItem(typeof(BootsOfBalladry), 25),
			new LootPackItem(typeof(BootsOfFleetness), 25),
			new LootPackItem(typeof(BootsOfSwiftness), 25),
			new LootPackItem(typeof(BootsOfTheNetherTraveller), 25),
			new LootPackItem(typeof(CarpentersCrown), 25),
			new LootPackItem(typeof(CelesRunebladeBuckler), 25),
			new LootPackItem(typeof(CetrasBlessing), 25),
			new LootPackItem(typeof(ChefsHatOfFocus), 25),
			new LootPackItem(typeof(CourtesansDaintyBuckler), 25),
			new LootPackItem(typeof(CourtesansFlowingRobe), 25),
			new LootPackItem(typeof(CourtesansGracefulHelm), 25),
			new LootPackItem(typeof(CourtesansWhisperingBoots), 25),
			new LootPackItem(typeof(CourtesansWhisperingGloves), 25),
			new LootPackItem(typeof(CourtierDashingBoots), 25),
			new LootPackItem(typeof(CourtiersEnchantedAmulet), 25),
			new LootPackItem(typeof(CourtierSilkenRobe), 25),
			new LootPackItem(typeof(CourtiersRegalCirclet), 25),
			new LootPackItem(typeof(CovensShadowedHood), 25),
			new LootPackItem(typeof(CreepersLeatherCap), 25),
			new LootPackItem(typeof(CrownOfTheAbyss), 25),
			new LootPackItem(typeof(DaedricWarHelm), 25),
			new LootPackItem(typeof(DarkFathersCrown), 25),
			new LootPackItem(typeof(DarkFathersDreadnaughtBoots), 25),
			new LootPackItem(typeof(DarkFathersHeartplate), 25),
			new LootPackItem(typeof(DarkFathersSoulGauntlets), 25),
			new LootPackItem(typeof(DarkFathersVoidLeggings), 25),
			new LootPackItem(typeof(DarkKnightsCursedChestplate), 25),
			new LootPackItem(typeof(DarkKnightsDoomShield), 25),
			new LootPackItem(typeof(DarkKnightsObsidianHelm), 25),
			new LootPackItem(typeof(DarkKnightsShadowedGauntlets), 25),
			new LootPackItem(typeof(DarkKnightsVoidLeggings), 25),
			new LootPackItem(typeof(DemonspikeGuard), 25),
			new LootPackItem(typeof(DespairsShadow), 25),
			new LootPackItem(typeof(Doombringer), 25),
			new LootPackItem(typeof(DragonbornChestplate), 25),
			new LootPackItem(typeof(DragonsBulwark), 25),
			new LootPackItem(typeof(DragoonsAegis), 25),
			new LootPackItem(typeof(DwemerAegis), 25),
			new LootPackItem(typeof(EbonyChainArms), 25),
			new LootPackItem(typeof(EdgarsEngineerChainmail), 25),
			new LootPackItem(typeof(EldarRuneGuard), 25),
			new LootPackItem(typeof(ElixirProtector), 25),
			new LootPackItem(typeof(EmberPlateArms), 25),
			new LootPackItem(typeof(EnderGuardiansChestplate), 25),
			new LootPackItem(typeof(ExodusBarrier), 25),
			new LootPackItem(typeof(FalconersCoif), 25),
			new LootPackItem(typeof(FlamePlateGorget), 25),
			new LootPackItem(typeof(FortunesGorget), 25),
			new LootPackItem(typeof(FortunesHelm), 25),
			new LootPackItem(typeof(FortunesPlateArms), 25),
			new LootPackItem(typeof(FortunesPlateChest), 25),
			new LootPackItem(typeof(FortunesPlateLegs), 25),
			new LootPackItem(typeof(FrostwardensBascinet), 25),
			new LootPackItem(typeof(FrostwardensPlateChest), 25),
			new LootPackItem(typeof(FrostwardensPlateGloves), 25),
			new LootPackItem(typeof(FrostwardensPlateLegs), 25),
			new LootPackItem(typeof(FrostwardensWoodenShield), 25),
			new LootPackItem(typeof(GauntletsOfPrecision), 25),
			new LootPackItem(typeof(GauntletsOfPurity), 25),
			new LootPackItem(typeof(GauntletsOfTheWild), 25),
			new LootPackItem(typeof(GloomfangChain), 25),
			new LootPackItem(typeof(GlovesOfTheSilentAssassin), 25),
			new LootPackItem(typeof(GlovesOfTransmutation), 25),
			new LootPackItem(typeof(GoronsGauntlets), 25),
			new LootPackItem(typeof(GreyWanderersStride), 25),
			new LootPackItem(typeof(GuardianAngelArms), 25),
			new LootPackItem(typeof(GuardianOfTheAbyss), 25),
			new LootPackItem(typeof(GuardiansHeartplate), 25),
			new LootPackItem(typeof(GuardiansHelm), 25),
			new LootPackItem(typeof(HammerlordsArmguards), 25),
			new LootPackItem(typeof(HammerlordsChestplate), 25),
			new LootPackItem(typeof(HammerlordsHelm), 25),
			new LootPackItem(typeof(HammerlordsLegplates), 25),
			new LootPackItem(typeof(HammerlordsShield), 25),
			new LootPackItem(typeof(HarmonyGauntlets), 25),
			new LootPackItem(typeof(HarmonysGuard), 25),
			new LootPackItem(typeof(HarvestersFootsteps), 25),
			new LootPackItem(typeof(HarvestersGrasp), 25),
			new LootPackItem(typeof(HarvestersGuard), 25),
			new LootPackItem(typeof(HarvestersHelm), 25),
			new LootPackItem(typeof(HarvestersStride), 25),
			new LootPackItem(typeof(HexweaversMysticalGloves), 25),
			new LootPackItem(typeof(HlaaluTradersCuffs), 25),
			new LootPackItem(typeof(HyruleKnightsShield), 25),
			new LootPackItem(typeof(ImmortalKingsIronCrown), 25),
			new LootPackItem(typeof(InfernoPlateChest), 25),
			new LootPackItem(typeof(InquisitorsGuard), 25),
			new LootPackItem(typeof(IstarisTouch), 25),
			new LootPackItem(typeof(JestersGleefulGloves), 25),
			new LootPackItem(typeof(JestersMerryCap), 25),
			new LootPackItem(typeof(JestersMischievousBuckler), 25),
			new LootPackItem(typeof(JestersPlayfulTunic), 25),
			new LootPackItem(typeof(JestersTricksterBoots), 25),
			new LootPackItem(typeof(KnightsAegis), 25),
			new LootPackItem(typeof(KnightsValorShield), 25),
			new LootPackItem(typeof(LeggingsOfTheRighteous), 25),
			new LootPackItem(typeof(LioneyesRemorse), 25),
			new LootPackItem(typeof(LionheartPlate), 25),
			new LootPackItem(typeof(LockesAdventurerLeather), 25),
			new LootPackItem(typeof(LocksleyLeatherChest), 25),
			new LootPackItem(typeof(LyricalGreaves), 25),
			new LootPackItem(typeof(LyricistsInsight), 25),
			new LootPackItem(typeof(MagitekInfusedPlate), 25),
			new LootPackItem(typeof(MakoResonance), 25),
			new LootPackItem(typeof(MaskedAvengersAgility), 25),
			new LootPackItem(typeof(MaskedAvengersDefense), 25),
			new LootPackItem(typeof(MaskedAvengersFocus), 25),
			new LootPackItem(typeof(MaskedAvengersPrecision), 25),
			new LootPackItem(typeof(MaskedAvengersVoice), 25),
			new LootPackItem(typeof(MelodicCirclet), 25),
			new LootPackItem(typeof(MerryMensStuddedGloves), 25),
			new LootPackItem(typeof(MeteorWard), 25),
			new LootPackItem(typeof(MinersHelmet), 25),
			new LootPackItem(typeof(MinstrelsMelody), 25),
			new LootPackItem(typeof(MisfortunesChains), 25),
			new LootPackItem(typeof(MondainsSkull), 25),
			new LootPackItem(typeof(MonksBattleWraps), 25),
			new LootPackItem(typeof(MonksSoulGloves), 25),
			new LootPackItem(typeof(MysticSeersPlate), 25),
			new LootPackItem(typeof(MysticsGuard), 25),
			new LootPackItem(typeof(NajsArcaneVestment), 25),
			new LootPackItem(typeof(NaturesEmbraceBelt), 25),
			new LootPackItem(typeof(NaturesEmbraceHelm), 25),
			new LootPackItem(typeof(NaturesGuardBoots), 25),
			new LootPackItem(typeof(NecklaceOfAromaticProtection), 25),
			new LootPackItem(typeof(NecromancersBoneGrips), 25),
			new LootPackItem(typeof(NecromancersDarkLeggings), 25),
			new LootPackItem(typeof(NecromancersHood), 25),
			new LootPackItem(typeof(NecromancersRobe), 25),
			new LootPackItem(typeof(NecromancersShadowBoots), 25),
			new LootPackItem(typeof(NightingaleVeil), 25),
			new LootPackItem(typeof(NinjaWrappings), 25),
			new LootPackItem(typeof(NottinghamStalkersLeggings), 25),
			new LootPackItem(typeof(OrkArdHat), 25),
			new LootPackItem(typeof(OutlawsForestBuckler), 25),
			new LootPackItem(typeof(PhilosophersGreaves), 25),
			new LootPackItem(typeof(PyrePlateHelm), 25),
			new LootPackItem(typeof(RadiantCrown), 25),
			new LootPackItem(typeof(RatsNest), 25),
			new LootPackItem(typeof(ReconnaissanceBoots), 25),
			new LootPackItem(typeof(RedoranDefendersGreaves), 25),
			new LootPackItem(typeof(RedstoneArtificersGloves), 25),
			new LootPackItem(typeof(RiotDefendersShield), 25),
			new LootPackItem(typeof(RoguesShadowBoots), 25),
			new LootPackItem(typeof(RoguesStealthShield), 25),
			new LootPackItem(typeof(RoyalCircletHelm), 25),
			new LootPackItem(typeof(SabatonsOfDawn), 25),
			new LootPackItem(typeof(SerenadesEmbrace), 25),
			new LootPackItem(typeof(SerpentScaleArmor), 25),
			new LootPackItem(typeof(SerpentsEmbrace), 25),
			new LootPackItem(typeof(ShadowGripGloves), 25),
			new LootPackItem(typeof(ShaftstopArmor), 25),
			new LootPackItem(typeof(ShaminosGreaves), 25),
			new LootPackItem(typeof(SherwoodArchersCap), 25),
			new LootPackItem(typeof(ShinobiHood), 25),
			new LootPackItem(typeof(ShurikenBracers), 25),
			new LootPackItem(typeof(SilentStepTabi), 25),
			new LootPackItem(typeof(SilksOfTheVictor), 25),
			new LootPackItem(typeof(SirensLament), 25),
			new LootPackItem(typeof(SirensResonance), 25),
			new LootPackItem(typeof(SkinOfTheVipermagi), 25),
			new LootPackItem(typeof(SlitheringSeal), 25),
			new LootPackItem(typeof(SolarisAegis), 25),
			new LootPackItem(typeof(SolarisLorica), 25),
			new LootPackItem(typeof(SOLDIERSMight), 25),
			new LootPackItem(typeof(SorrowsGrasp), 25),
			new LootPackItem(typeof(StealthOperatorsGear), 25),
			new LootPackItem(typeof(StormcrowsGaze), 25),
			new LootPackItem(typeof(StormforgedBoots), 25),
			new LootPackItem(typeof(StormforgedGauntlets), 25),
			new LootPackItem(typeof(StormforgedHelm), 25),
			new LootPackItem(typeof(StormforgedLeggings), 25),
			new LootPackItem(typeof(StormforgedPlateChest), 25),
			new LootPackItem(typeof(Stormshield), 25),
			new LootPackItem(typeof(StringOfEars), 25),
			new LootPackItem(typeof(SummonersEmbrace), 25),
			new LootPackItem(typeof(TabulaRasa), 25),
			new LootPackItem(typeof(TacticalVest), 25),
			new LootPackItem(typeof(TailorsTouch), 25),
			new LootPackItem(typeof(TalsRashasRelic), 25),
			new LootPackItem(typeof(TamersBindings), 25),
			new LootPackItem(typeof(TechPriestMantle), 25),
			new LootPackItem(typeof(TelvanniMagistersCap), 25),
			new LootPackItem(typeof(TerrasMysticRobe), 25),
			new LootPackItem(typeof(TheThinkingCap), 25),
			new LootPackItem(typeof(ThiefsNimbleCap), 25),
			new LootPackItem(typeof(ThievesGuildPants), 25),
			new LootPackItem(typeof(ThundergodsVigor), 25),
			new LootPackItem(typeof(TinkersTreads), 25),
			new LootPackItem(typeof(ToxinWard), 25),
			new LootPackItem(typeof(TunicOfTheWild), 25),
			new LootPackItem(typeof(TyraelsVigil), 25),
			new LootPackItem(typeof(ValkyriesWard), 25),
			new LootPackItem(typeof(VeilOfSteel), 25),
			new LootPackItem(typeof(Venomweave), 25),
			new LootPackItem(typeof(VialWarden), 25),
			new LootPackItem(typeof(VipersCoif), 25),
			new LootPackItem(typeof(VirtueGuard), 25),
			new LootPackItem(typeof(VortexMantle), 25),
			new LootPackItem(typeof(VyrsGraspingGauntlets), 25),
			new LootPackItem(typeof(WardensAegis), 25),
			new LootPackItem(typeof(WhispersHeartguard), 25),
			new LootPackItem(typeof(WhiteMagesDivineVestment), 25),
			new LootPackItem(typeof(WhiteRidersGuard), 25),
			new LootPackItem(typeof(WhiteSageCap), 25),
			new LootPackItem(typeof(WildwalkersGreaves), 25),
			new LootPackItem(typeof(WinddancerBoots), 25),
			new LootPackItem(typeof(WisdomsCirclet), 25),
			new LootPackItem(typeof(WisdomsEmbrace), 25),
			new LootPackItem(typeof(WitchesBindingGloves), 25),
			new LootPackItem(typeof(WitchesCursedRobe), 25),
			new LootPackItem(typeof(WitchesEnchantedHat), 25),
			new LootPackItem(typeof(WitchesEnchantedRobe), 25),
			new LootPackItem(typeof(WitchesHeartAmulet), 25),
			new LootPackItem(typeof(WitchesWhisperingBoots), 25),
			new LootPackItem(typeof(WitchfireShield), 25),
			new LootPackItem(typeof(WitchwoodGreaves), 25),
			new LootPackItem(typeof(WraithsBane), 25),
			new LootPackItem(typeof(WrestlersArmsOfPrecision), 25),
			new LootPackItem(typeof(WrestlersChestOfPower), 25),
			new LootPackItem(typeof(WrestlersGrippingGloves), 25),
			new LootPackItem(typeof(WrestlersHelmOfFocus), 25),
			new LootPackItem(typeof(WrestlersLeggingsOfBalance), 25),
			new LootPackItem(typeof(ZorasFins), 25),
			new LootPackItem(typeof(AdventurersBoots), 25),
			new LootPackItem(typeof(AerobicsInstructorsLegwarmers), 25),
			new LootPackItem(typeof(AmbassadorsCloak), 25),
			new LootPackItem(typeof(AnglersSeabreezeCloak), 25),
			new LootPackItem(typeof(ArchivistsShoes), 25),
			new LootPackItem(typeof(ArrowsmithsSturdyBoots), 25),
			new LootPackItem(typeof(ArtisansTimberShoes), 25),
			new LootPackItem(typeof(AssassinsBandana), 25),
			new LootPackItem(typeof(AssassinsMaskedCap), 25),
			new LootPackItem(typeof(BaggyHipHopPants), 25),
			new LootPackItem(typeof(BakersSoftShoes), 25),
			new LootPackItem(typeof(BalladeersMuffler), 25),
			new LootPackItem(typeof(BanditsHiddenCloak), 25),
			new LootPackItem(typeof(BardOfErinsMuffler), 25),
			new LootPackItem(typeof(BardsTunicOfStonehenge), 25),
			new LootPackItem(typeof(BaristasMuffler), 25),
			new LootPackItem(typeof(BeastmastersTanic), 25),
			new LootPackItem(typeof(BeastmastersTonic), 25),
			new LootPackItem(typeof(BeastmastersTunic), 25),
			new LootPackItem(typeof(BeastmiastersTunic), 25),
			new LootPackItem(typeof(BeatniksBeret), 25),
			new LootPackItem(typeof(BeggarsLuckyBandana), 25),
			new LootPackItem(typeof(BlacksmithsReinforcedGloves), 25),
			new LootPackItem(typeof(BobbySoxersShoes), 25),
			new LootPackItem(typeof(BohoChicSundress), 25),
			new LootPackItem(typeof(BootsOfTheDeepCaverns), 25),
			new LootPackItem(typeof(BowcraftersProtectiveCloak), 25),
			new LootPackItem(typeof(BowyersInsightfulBandana), 25),
			new LootPackItem(typeof(BreakdancersCap), 25),
			new LootPackItem(typeof(CarpentersStalwartTunic), 25),
			new LootPackItem(typeof(CartographersExploratoryTunic), 25),
			new LootPackItem(typeof(CartographersHat), 25),
			new LootPackItem(typeof(CeltidDruidsRobe), 25),
			new LootPackItem(typeof(ChampagneToastTunic), 25),
			new LootPackItem(typeof(ChefsGourmetApron), 25),
			new LootPackItem(typeof(ClericsSacredSash), 25),
			new LootPackItem(typeof(CourtesansGracefulKimono), 25),
			new LootPackItem(typeof(CourtisansRefinedGown), 25),
			new LootPackItem(typeof(CouturiersSundress), 25),
			new LootPackItem(typeof(CraftsmansProtectiveGloves), 25),
			new LootPackItem(typeof(CropTopMystic), 25),
			new LootPackItem(typeof(CuratorsKilt), 25),
			new LootPackItem(typeof(CyberpunkNinjaTabi), 25),
			new LootPackItem(typeof(DancersEnchantedSkirt), 25),
			new LootPackItem(typeof(DapperFedoraOfInsight), 25),
			new LootPackItem(typeof(DarkLordsRobe), 25),
			new LootPackItem(typeof(DataMagesDigitalCloak), 25),
			new LootPackItem(typeof(DeepSeaTunic), 25),
			new LootPackItem(typeof(DenimJacketOfReflection), 25),
			new LootPackItem(typeof(DiplomatsTunic), 25),
			new LootPackItem(typeof(DiscoDivaBoots), 25),
			new LootPackItem(typeof(ElementalistsProtectiveCloak), 25),
			new LootPackItem(typeof(ElvenSnowBoots), 25),
			new LootPackItem(typeof(EmoSceneHairpin), 25),
			new LootPackItem(typeof(ExplorersBoots), 25),
			new LootPackItem(typeof(FilmNoirDetectivesTrenchCoat), 25),
			new LootPackItem(typeof(FishermansSunHat), 25),
			new LootPackItem(typeof(FishermansVest), 25),
			new LootPackItem(typeof(FishmongersKilt), 25),
			new LootPackItem(typeof(FletchersPrecisionGloves), 25),
			new LootPackItem(typeof(FlowerChildSundress), 25),
			new LootPackItem(typeof(ForestersTunic), 25),
			new LootPackItem(typeof(ForgeMastersBoots), 25),
			new LootPackItem(typeof(GazeCapturingVeil), 25),
			new LootPackItem(typeof(GeishasGracefulKasa), 25),
			new LootPackItem(typeof(GhostlyShroud), 25),
			new LootPackItem(typeof(GlamRockersJacket), 25),
			new LootPackItem(typeof(GlovesOfStonemasonry), 25),
			new LootPackItem(typeof(GoGoBootsOfAgility), 25),
			new LootPackItem(typeof(GrapplersTunic), 25),
			new LootPackItem(typeof(GreenwichMagesRobe), 25),
			new LootPackItem(typeof(GroovyBellBottomPants), 25),
			new LootPackItem(typeof(GrungeBandana), 25),
			new LootPackItem(typeof(HackersVRGoggles), 25),
			new LootPackItem(typeof(HammerlordsCap), 25),
			new LootPackItem(typeof(HarmonistsSoftShoes), 25),
			new LootPackItem(typeof(HealersBlessedSandals), 25),
			new LootPackItem(typeof(HealersFurCape), 25),
			new LootPackItem(typeof(HelmetOfTheOreWhisperer), 25),
			new LootPackItem(typeof(HerbalistsProtectiveHat), 25),
			new LootPackItem(typeof(HerdersMuffler), 25),
			new LootPackItem(typeof(HippiePeaceBandana), 25),
			new LootPackItem(typeof(HippiesPeacefulSandals), 25),
			new LootPackItem(typeof(IntriguersFeatheredHat), 25),
			new LootPackItem(typeof(JazzMusiciansMuffler), 25),
			new LootPackItem(typeof(KnightsHelmOfTheRoundTable), 25),
			new LootPackItem(typeof(LeprechaunsLuckyHat), 25),
			new LootPackItem(typeof(LorekeepersSash), 25),
			new LootPackItem(typeof(LuchadorsMask), 25),
			new LootPackItem(typeof(MapmakersInsightfulMuffler), 25),
			new LootPackItem(typeof(MarinersLuckyBoots), 25),
			new LootPackItem(typeof(MelodiousMuffler), 25),
			new LootPackItem(typeof(MendersDivineRobe), 25),
			new LootPackItem(typeof(MidnightRevelersBoots), 25),
			new LootPackItem(typeof(MinersSturdyBoots), 25),
			new LootPackItem(typeof(MinstrelsTunedTunic), 25),
			new LootPackItem(typeof(MistletoeMuffler), 25),
			new LootPackItem(typeof(ModStyleTunic), 25),
			new LootPackItem(typeof(MoltenCloak), 25),
			new LootPackItem(typeof(MonksMeditativeRobe), 25),
			new LootPackItem(typeof(MummysWrappings), 25),
			new LootPackItem(typeof(MysticsFeatheredHat), 25),
			new LootPackItem(typeof(NaturalistsCloak), 25),
			new LootPackItem(typeof(NaturesMuffler), 25),
			new LootPackItem(typeof(NavigatorsProtectiveCap), 25),
			new LootPackItem(typeof(NecromancersCape), 25),
			new LootPackItem(typeof(NeonStreetSash), 25),
			new LootPackItem(typeof(NewWaveNeonShades), 25),
			new LootPackItem(typeof(NinjasKasa), 25),
			new LootPackItem(typeof(NinjasStealthyTabi), 25),
			new LootPackItem(typeof(OreSeekersBandana), 25),
			new LootPackItem(typeof(PickpocketsNimbleGloves), 25),
			new LootPackItem(typeof(PickpocketsSleekTunic), 25),
			new LootPackItem(typeof(PinUpHalterDress), 25),
			new LootPackItem(typeof(PlatformSneakers), 25),
			new LootPackItem(typeof(PoodleSkirtOfCharm), 25),
			new LootPackItem(typeof(PopStarsFingerlessGloves), 25),
			new LootPackItem(typeof(PopStarsGlitteringCap), 25),
			new LootPackItem(typeof(PopStarsSparklingBandana), 25),
			new LootPackItem(typeof(PreserversCap), 25),
			new LootPackItem(typeof(PsychedelicTieDyeShirt), 25),
			new LootPackItem(typeof(PsychedelicWizardsHat), 25),
			new LootPackItem(typeof(PumpkinKingsCrown), 25),
			new LootPackItem(typeof(QuivermastersTunic), 25),
			new LootPackItem(typeof(RangersCap), 25),
			new LootPackItem(typeof(RangersHat), 25),
			new LootPackItem(typeof(RangersHatNightSight), 25),
			new LootPackItem(typeof(ReindeerFurCap), 25),
			new LootPackItem(typeof(ResolutionKeepersSash), 25),
			new LootPackItem(typeof(RingmastersSandals), 25),
			new LootPackItem(typeof(RockabillyRebelJacket), 25),
			new LootPackItem(typeof(RoguesDeceptiveMask), 25),
			new LootPackItem(typeof(RoguesShadowCloak), 25),
			new LootPackItem(typeof(RoyalGuardsBoots), 25),
			new LootPackItem(typeof(SamuraisHonorableTunic), 25),
			new LootPackItem(typeof(SantasEnchantedRobe), 25),
			new LootPackItem(typeof(SawyersMightyApron), 25),
			new LootPackItem(typeof(ScholarsRobe), 25),
			new LootPackItem(typeof(ScoutsWideBrimHat), 25),
			new LootPackItem(typeof(ScribersRobe), 25),
			new LootPackItem(typeof(ScribesEnlightenedSandals), 25),
			new LootPackItem(typeof(ScriptoriumMastersRobe), 25),
			new LootPackItem(typeof(SeductressSilkenShoes), 25),
			new LootPackItem(typeof(SeersMysticSash), 25),
			new LootPackItem(typeof(ShadowWalkersTabi), 25),
			new LootPackItem(typeof(ShanachiesStorytellingShoes), 25),
			new LootPackItem(typeof(ShepherdsKilt), 25),
			new LootPackItem(typeof(SherlocksSleuthingCap), 25),
			new LootPackItem(typeof(ShogunsAuthoritativeSurcoat), 25),
			new LootPackItem(typeof(SilentNightCloak), 25),
			new LootPackItem(typeof(SkatersBaggyPants), 25),
			new LootPackItem(typeof(SmithsProtectiveTunic), 25),
			new LootPackItem(typeof(SneaksSilentShoes), 25),
			new LootPackItem(typeof(SnoopersSoftGloves), 25),
			new LootPackItem(typeof(SommelierBodySash), 25),
			new LootPackItem(typeof(SorceressMidnightRobe), 25),
			new LootPackItem(typeof(SpellweaversEnchantedShoes), 25),
			new LootPackItem(typeof(StarletsFancyDress), 25),
			new LootPackItem(typeof(StarlightWizardsHat), 25),
			new LootPackItem(typeof(StarlightWozardsHat), 25),
			new LootPackItem(typeof(StreetArtistsBaggyPants), 25),
			new LootPackItem(typeof(StreetPerformersCap), 25),
			new LootPackItem(typeof(SubmissionsArtistsMuffler), 25),
			new LootPackItem(typeof(SurgeonsInsightfulMask), 25),
			new LootPackItem(typeof(SwingsDancersShoes), 25),
			new LootPackItem(typeof(TailorsFancyApron), 25),
			new LootPackItem(typeof(TamersKilt), 25),
			new LootPackItem(typeof(TamersMuffler), 25),
			new LootPackItem(typeof(TechGurusGlasses), 25),
			new LootPackItem(typeof(TechnomancersHoodie), 25),
			new LootPackItem(typeof(ThiefsShadowTunic), 25),
			new LootPackItem(typeof(ThiefsSilentShoes), 25),
			new LootPackItem(typeof(TidecallersSandals), 25),
			new LootPackItem(typeof(TruckersIconicCap), 25),
			new LootPackItem(typeof(UrbanitesSneakers), 25),
			new LootPackItem(typeof(VampiresMidnightCloak), 25),
			new LootPackItem(typeof(VestOfTheVeinSeeker), 25),
			new LootPackItem(typeof(WarHeronsCap), 25),
			new LootPackItem(typeof(WarriorOfUlstersTunic), 25),
			new LootPackItem(typeof(WarriorsBelt), 25),
			new LootPackItem(typeof(WhisperersBoots), 25),
			new LootPackItem(typeof(WhisperersSandals), 25),
			new LootPackItem(typeof(WhisperingSandals), 25),
			new LootPackItem(typeof(WhisperingSondals), 25),
			new LootPackItem(typeof(WhisperingWindSash), 25),
			new LootPackItem(typeof(WitchesBewitchingRobe), 25),
			new LootPackItem(typeof(WitchesBrewedHat), 25),
			new LootPackItem(typeof(WoodworkersInsightfulCap), 25),
			new LootPackItem(typeof(AegisShield), 25),
			new LootPackItem(typeof(AeonianBow), 25),
			new LootPackItem(typeof(AlamoDefendersAxe), 25),
			new LootPackItem(typeof(AlucardsBlade), 25),
			new LootPackItem(typeof(AnubisWarMace), 25),
			new LootPackItem(typeof(ApepsCoiledScimitar), 25),
			new LootPackItem(typeof(ApollosSong), 25),
			new LootPackItem(typeof(ArchersYewBow), 25),
			new LootPackItem(typeof(AssassinsKryss), 25),
			new LootPackItem(typeof(AtmaBlade), 25),
			new LootPackItem(typeof(AxeOfTheJuggernaut), 25),
			new LootPackItem(typeof(AxeOfTheRuneweaver), 25),
			new LootPackItem(typeof(BaneOfTheDead), 25),
			new LootPackItem(typeof(BanshoFanClub), 25),
			new LootPackItem(typeof(BarbarossaScimitar), 25),
			new LootPackItem(typeof(BardsBowOfDiscord), 25),
			new LootPackItem(typeof(BeowulfsWarAxe), 25),
			new LootPackItem(typeof(BismarckianWarAxe), 25),
			new LootPackItem(typeof(Blackrazor), 25),
			new LootPackItem(typeof(BlacksmithsWarHammer), 25),
			new LootPackItem(typeof(BlackSwordOfMondain), 25),
			new LootPackItem(typeof(BlackTailWhip), 25),
			new LootPackItem(typeof(BladeOfTheStars), 25),
			new LootPackItem(typeof(Bonehew), 25),
			new LootPackItem(typeof(BowiesLegacy), 25),
			new LootPackItem(typeof(BowOfAuriel), 25),
			new LootPackItem(typeof(BowOfIsrafil), 25),
			new LootPackItem(typeof(BowspritOfBluenose), 25),
			new LootPackItem(typeof(BulKathosTribalGuardian), 25),
			new LootPackItem(typeof(BusterSwordReplica), 25),
			new LootPackItem(typeof(ButchersCleaver), 25),
			new LootPackItem(typeof(CaduceusStaff), 25),
			new LootPackItem(typeof(CelestialLongbow), 25),
			new LootPackItem(typeof(CelestialScimitar), 25),
			new LootPackItem(typeof(CetrasStaff), 25),
			new LootPackItem(typeof(ChakramBlade), 25),
			new LootPackItem(typeof(CharlemagnesWarAxe), 25),
			new LootPackItem(typeof(CherubsBlade), 25),
			new LootPackItem(typeof(ChillrendLongsword), 25),
			new LootPackItem(typeof(ChuKoNu), 25),
			new LootPackItem(typeof(CrissaegrimEdge), 25),
			new LootPackItem(typeof(CthulhusGaze), 25),
			new LootPackItem(typeof(CursedArmorCleaver), 25),
			new LootPackItem(typeof(CustersLastStandBow), 25),
			new LootPackItem(typeof(DaggerOfShadows), 25),
			new LootPackItem(typeof(DavidsSling), 25),
			new LootPackItem(typeof(DawnbreakerMace), 25),
			new LootPackItem(typeof(DeadMansLegacy), 25),
			new LootPackItem(typeof(DestructoDiscDagger), 25),
			new LootPackItem(typeof(DianasMoonBow), 25),
			new LootPackItem(typeof(DoomfletchsPrism), 25),
			new LootPackItem(typeof(Doomsickle), 25),
			new LootPackItem(typeof(DragonClaw), 25),
			new LootPackItem(typeof(DragonsBreath), 25),
			new LootPackItem(typeof(DragonsBreathWarAxe), 25),
			new LootPackItem(typeof(DragonsScaleDagger), 25),
			new LootPackItem(typeof(DragonsWrath), 25),
			new LootPackItem(typeof(Dreamseeker), 25),
			new LootPackItem(typeof(EarthshakerMaul), 25),
			new LootPackItem(typeof(EbonyWarAxeOfVampires), 25),
			new LootPackItem(typeof(EldritchBowOfShadows), 25),
			new LootPackItem(typeof(EldritchWhisper), 25),
			new LootPackItem(typeof(ErdricksBlade), 25),
			new LootPackItem(typeof(Excalibur), 25),
			new LootPackItem(typeof(ExcaliburLongsword), 25),
			new LootPackItem(typeof(ExcalibursLegacy), 25),
			new LootPackItem(typeof(FangOfStorms), 25),
			new LootPackItem(typeof(FlamebaneWarAxe), 25),
			new LootPackItem(typeof(FrostfireCleaver), 25),
			new LootPackItem(typeof(FrostflameKatana), 25),
			new LootPackItem(typeof(FuHaosBattleAxe), 25),
			new LootPackItem(typeof(GenjiBow), 25),
			new LootPackItem(typeof(GeomancersStaff), 25),
			new LootPackItem(typeof(GhoulSlayersLongsword), 25),
			new LootPackItem(typeof(GlassSword), 25),
			new LootPackItem(typeof(GlassSwordOfValor), 25),
			new LootPackItem(typeof(GoldbrandScimitar), 25),
			new LootPackItem(typeof(GreenDragonCrescentBlade), 25),
			new LootPackItem(typeof(Grimmblade), 25),
			new LootPackItem(typeof(GrimReapersCleaver), 25),
			new LootPackItem(typeof(GriswoldsEdge), 25),
			new LootPackItem(typeof(GrognaksAxe), 25),
			new LootPackItem(typeof(GuardianOfTheFey), 25),
			new LootPackItem(typeof(GuillotineBladeDagger), 25),
			new LootPackItem(typeof(HalberdOfHonesty), 25),
			new LootPackItem(typeof(HanseaticCrossbow), 25),
			new LootPackItem(typeof(HarmonyBow), 25),
			new LootPackItem(typeof(HarpeBlade), 25),
			new LootPackItem(typeof(HeartbreakerSunder), 25),
			new LootPackItem(typeof(HelmOfDarkness), 25),
			new LootPackItem(typeof(IlluminaDagger), 25),
			new LootPackItem(typeof(InuitUluOfTheNorth), 25),
			new LootPackItem(typeof(JoansDivineLongsword), 25),
			new LootPackItem(typeof(JuggernautHammer), 25),
			new LootPackItem(typeof(KaomsCleaver), 25),
			new LootPackItem(typeof(KaomsMaul), 25),
			new LootPackItem(typeof(Keenstrike), 25),
			new LootPackItem(typeof(KhufusWarSpear), 25),
			new LootPackItem(typeof(KingsSwordOfHaste), 25),
			new LootPackItem(typeof(MaatsBalancedBow), 25),
			new LootPackItem(typeof(MablungsDefender), 25),
			new LootPackItem(typeof(MaceOfTheVoid), 25),
			new LootPackItem(typeof(MageMasher), 25),
			new LootPackItem(typeof(MageMusher), 25),
			new LootPackItem(typeof(MagesStaff), 25),
			new LootPackItem(typeof(MagicAxeOfGreatStrength), 25),
			new LootPackItem(typeof(MagusRod), 25),
			new LootPackItem(typeof(MakhairaOfAchilles), 25),
			new LootPackItem(typeof(ManajumasKnife), 25),
			new LootPackItem(typeof(MarssBattleAxeOfValor), 25),
			new LootPackItem(typeof(MasamuneBlade), 25),
			new LootPackItem(typeof(MasamuneKatana), 25),
			new LootPackItem(typeof(MasamunesEdge), 25),
			new LootPackItem(typeof(MasamunesGrace), 25),
			new LootPackItem(typeof(MaulOfSulayman), 25),
			new LootPackItem(typeof(MehrunesCleaver), 25),
			new LootPackItem(typeof(MortuarySword), 25),
			new LootPackItem(typeof(MosesStaff), 25),
			new LootPackItem(typeof(MuramasasBloodlust), 25),
			new LootPackItem(typeof(MusketeersRapier), 25),
			new LootPackItem(typeof(MysticBowOfLight), 25),
			new LootPackItem(typeof(MysticStaffOfElements), 25),
			new LootPackItem(typeof(NaginataOfTomoeGozen), 25),
			new LootPackItem(typeof(NebulaBow), 25),
			new LootPackItem(typeof(NecromancersDagger), 25),
			new LootPackItem(typeof(NeptunesTrident), 25),
			new LootPackItem(typeof(NormanConquerorsBow), 25),
			new LootPackItem(typeof(PaladinsChrysblade), 25),
			new LootPackItem(typeof(PlasmaInfusedWarHammer), 25),
			new LootPackItem(typeof(PlutosAbyssalMace), 25),
			new LootPackItem(typeof(PotaraEarringClub), 25),
			new LootPackItem(typeof(PowerPoleHalberd), 25),
			new LootPackItem(typeof(PowersBeacon), 25),
			new LootPackItem(typeof(ProhibitionClub), 25),
			new LootPackItem(typeof(QamarDagger), 25),
			new LootPackItem(typeof(QuasarAxe), 25),
			new LootPackItem(typeof(RainbowBlade), 25),
			new LootPackItem(typeof(RasSearingDagger), 25),
			new LootPackItem(typeof(ReflectionShield), 25),
			new LootPackItem(typeof(RevolutionarySabre), 25),
			new LootPackItem(typeof(RielsRebellionSabre), 25),
			new LootPackItem(typeof(RuneAss), 25),
			new LootPackItem(typeof(RuneAxe), 25),
			new LootPackItem(typeof(SaiyanTailWhip), 25),
			new LootPackItem(typeof(SamsonsJawbone), 25),
			new LootPackItem(typeof(SaxonSeax), 25),
			new LootPackItem(typeof(SearingTouch), 25),
			new LootPackItem(typeof(SerpentsFang), 25),
			new LootPackItem(typeof(SerpentsVenomDagger), 25),
			new LootPackItem(typeof(ShadowstrideBow), 25),
			new LootPackItem(typeof(ShavronnesRapier), 25),
			new LootPackItem(typeof(SkyPiercer), 25),
			new LootPackItem(typeof(SoulTaker), 25),
			new LootPackItem(typeof(StaffOfAeons), 25),
			new LootPackItem(typeof(StaffOfApocalypse), 25),
			new LootPackItem(typeof(StaffOfRainsWrath), 25),
			new LootPackItem(typeof(StaffOfTheElements), 25),
			new LootPackItem(typeof(StarfallDagger), 25),
			new LootPackItem(typeof(Sunblade), 25),
			new LootPackItem(typeof(SwordOfAlBattal), 25),
			new LootPackItem(typeof(SwordOfGideon), 25),
			new LootPackItem(typeof(TabulasDagger), 25),
			new LootPackItem(typeof(TantoOfThe47Ronin), 25),
			new LootPackItem(typeof(TempestHammer), 25),
			new LootPackItem(typeof(TeutonicWarMace), 25),
			new LootPackItem(typeof(TheFurnace), 25),
			new LootPackItem(typeof(TheOculus), 25),
			new LootPackItem(typeof(ThorsHammer), 25),
			new LootPackItem(typeof(Thunderfury), 25),
			new LootPackItem(typeof(Thunderstroke), 25),
			new LootPackItem(typeof(TitansFury), 25),
			new LootPackItem(typeof(TomahawkOfTecumseh), 25),
			new LootPackItem(typeof(TouchOfAnguish), 25),
			new LootPackItem(typeof(TriLithiumBlade), 25),
			new LootPackItem(typeof(TwoShotCrossbow), 25),
			new LootPackItem(typeof(UltimaGlaive), 25),
			new LootPackItem(typeof(UmbraWarAxe), 25),
			new LootPackItem(typeof(UndeadCrown), 25),
			new LootPackItem(typeof(ValiantThrower), 25),
			new LootPackItem(typeof(VampireKiller), 25),
			new LootPackItem(typeof(VATSEnhancedDagger), 25),
			new LootPackItem(typeof(VenomsSting), 25),
			new LootPackItem(typeof(VoidsEmbrace), 25),
			new LootPackItem(typeof(VolendrungWarHammer), 25),
			new LootPackItem(typeof(VolendrungWorHammer), 25),
			new LootPackItem(typeof(VoltaxicRiftLance), 25),
			new LootPackItem(typeof(VoyageursPaddle), 25),
			new LootPackItem(typeof(VulcansForgeHammer), 25),
			new LootPackItem(typeof(WabbajackClub), 25),
			new LootPackItem(typeof(WandOfWoh), 25),
			new LootPackItem(typeof(Whelm), 25),
			new LootPackItem(typeof(WhisperingWindWarMace), 25),
			new LootPackItem(typeof(WhisperwindBow), 25),
			new LootPackItem(typeof(WindDancersDagger), 25),
			new LootPackItem(typeof(WindripperBow), 25),
			new LootPackItem(typeof(Wizardspike), 25),
			new LootPackItem(typeof(WondershotCrossbow), 25),
			new LootPackItem(typeof(Xcalibur), 25),
			new LootPackItem(typeof(YumiOfEmpressJingu), 25),
			new LootPackItem(typeof(ZhugeFeathersFan), 25),
			new LootPackItem(typeof(Zulfiqar), 25),
			new LootPackItem(typeof(AbbasidsTreasureChest), 25),
			new LootPackItem(typeof(AbyssalPlaneChest), 25),
			new LootPackItem(typeof(AlehouseChest), 25),
			new LootPackItem(typeof(AlienArtifactChest), 25),
			new LootPackItem(typeof(AlienArtifaxChest), 25),
			new LootPackItem(typeof(AlliedForcesTreasureChest), 25),
			new LootPackItem(typeof(AnarchistsCache), 25),
			new LootPackItem(typeof(AncientRelicChest), 25),
			new LootPackItem(typeof(AngelBlessingChest), 25),
			new LootPackItem(typeof(AnglersBounty), 25),
			new LootPackItem(typeof(ArcadeKingsTreasure), 25),
			new LootPackItem(typeof(ArcadeMastersVault), 25),
			new LootPackItem(typeof(ArcaneTreasureChest), 25),
			new LootPackItem(typeof(ArcanumChest), 25),
			new LootPackItem(typeof(ArcheryBonusChest), 25),
			new LootPackItem(typeof(AshokasTreasureChest), 25),
			new LootPackItem(typeof(AshokaTreasureChest), 25),
			new LootPackItem(typeof(AssassinsCoffer), 25),
			new LootPackItem(typeof(AthenianTreasureChest), 25),
			new LootPackItem(typeof(BabylonianChest), 25),
			new LootPackItem(typeof(BakersDelightChest), 25),
			new LootPackItem(typeof(BakersDolightChest), 25),
			new LootPackItem(typeof(BavarianFestChest), 25),
			new LootPackItem(typeof(BismarcksTreasureChest), 25),
			new LootPackItem(typeof(BolsheviksLoot), 25),
			new LootPackItem(typeof(BountyHuntersCache), 25),
			new LootPackItem(typeof(BoyBandBox), 25),
			new LootPackItem(typeof(BrewmastersChest), 25),
			new LootPackItem(typeof(BritainsRoyalTreasuryChest), 25),
			new LootPackItem(typeof(BuccaneersChest), 25),
			new LootPackItem(typeof(CaesarChest), 25),
			new LootPackItem(typeof(CandyCarnivalCoffer), 25),
			new LootPackItem(typeof(CaptainCooksTreasure), 25),
			new LootPackItem(typeof(CelticLegendsChest), 25),
			new LootPackItem(typeof(ChamplainTreasureChest), 25),
			new LootPackItem(typeof(CheeseConnoisseursCache), 25),
			new LootPackItem(typeof(ChocolatierTreasureChest), 25),
			new LootPackItem(typeof(CivilRightsStrongbox), 25),
			new LootPackItem(typeof(CivilWarCache), 25),
			new LootPackItem(typeof(CivilWarChest), 25),
			new LootPackItem(typeof(CivilWorChest), 25),
			new LootPackItem(typeof(ClownsWhimsicalChest), 25),
			new LootPackItem(typeof(ColonialPioneersCache), 25),
			new LootPackItem(typeof(ComradesCache), 25),
			new LootPackItem(typeof(ConfederationCache), 25),
			new LootPackItem(typeof(ConquistadorsHoard), 25),
			new LootPackItem(typeof(CovenTreasuresChest), 25),
			new LootPackItem(typeof(CyberneticCache), 25),
			new LootPackItem(typeof(CyrusTreasure), 25),
			new LootPackItem(typeof(DesertPharaohChest), 25),
			new LootPackItem(typeof(DinerDelightChest), 25),
			new LootPackItem(typeof(DoctorsBag), 25),
			new LootPackItem(typeof(DojoLegacyChest), 25),
			new LootPackItem(typeof(DragonGuardiansHoardChest), 25),
			new LootPackItem(typeof(DragonHoardChest), 25),
			new LootPackItem(typeof(DragonHoChest), 25),
			new LootPackItem(typeof(DragonHodChest), 25),
			new LootPackItem(typeof(DragonHorChest), 25),
			new LootPackItem(typeof(DriveInTreasureTrove), 25),
			new LootPackItem(typeof(DroidWorkshopChest), 25),
			new LootPackItem(typeof(DynastyRelicsChest), 25),
			new LootPackItem(typeof(EdisonsTreasureChest), 25),
			new LootPackItem(typeof(EgyptianChest), 25),
			new LootPackItem(typeof(EliteFoursVault), 25),
			new LootPackItem(typeof(ElvenEnchantressChest), 25),
			new LootPackItem(typeof(ElvenTreasuryChest), 25),
			new LootPackItem(typeof(EmeraldIsleChest), 25),
			new LootPackItem(typeof(EmperorJustinianCache), 25),
			new LootPackItem(typeof(EmperorLegacyChest), 25),
			new LootPackItem(typeof(EnchantedForestChest), 25),
			new LootPackItem(typeof(EtherealPlaneChest), 25),
			new LootPackItem(typeof(EuropeanRelicsChest), 25),
			new LootPackItem(typeof(FairyDustChest), 25),
			new LootPackItem(typeof(FirstNationsHeritageChest), 25),
			new LootPackItem(typeof(FlowerPowerChest), 25),
			new LootPackItem(typeof(FocusBonusChest), 25),
			new LootPackItem(typeof(ForbiddenAlchemistsCache), 25),
			new LootPackItem(typeof(FrontierExplorersStash), 25),
			new LootPackItem(typeof(FunkyFashionChest), 25),
			new LootPackItem(typeof(FurTradersChest), 25),
			new LootPackItem(typeof(GalacticExplorersTrove), 25),
			new LootPackItem(typeof(GalacticRelicsChest), 25),
			new LootPackItem(typeof(GamersLootbox), 25),
			new LootPackItem(typeof(GardenersParadiseChest), 25),
			new LootPackItem(typeof(GeishasGift), 25),
			new LootPackItem(typeof(GermanUnificationChest), 25),
			new LootPackItem(typeof(GoldRushBountyChest), 25),
			new LootPackItem(typeof(GoldRushRelicChest), 25),
			new LootPackItem(typeof(GreasersGoldmineChest), 25),
			new LootPackItem(typeof(GroovyVabesChest), 25),
			new LootPackItem(typeof(GroovyVibesChest), 25),
			new LootPackItem(typeof(GrungeRockersCache), 25),
			new LootPackItem(typeof(HipHopRapVault), 25),
			new LootPackItem(typeof(HolyRomanEmpireChest), 25),
			new LootPackItem(typeof(HomewardBoundChest), 25),
			new LootPackItem(typeof(HussarsChest), 25),
			new LootPackItem(typeof(InfernalPlaneChest), 25),
			new LootPackItem(typeof(InnovatorVault), 25),
			new LootPackItem(typeof(JedisReliquary), 25),
			new LootPackItem(typeof(JestersGigglingChest), 25),
			new LootPackItem(typeof(JestersJamboreeChest), 25),
			new LootPackItem(typeof(JestersJest), 25),
			new LootPackItem(typeof(JudahsTreasureChest), 25),
			new LootPackItem(typeof(JukeboxJewels), 25),
			new LootPackItem(typeof(JustinianTreasureChest), 25),
			new LootPackItem(typeof(KagesTreasureChest), 25),
			new LootPackItem(typeof(KingdomsVaultChest), 25),
			new LootPackItem(typeof(KingKamehamehaTreasure), 25),
			new LootPackItem(typeof(KingsBest), 25),
			new LootPackItem(typeof(KoscheisUndyingChest), 25),
			new LootPackItem(typeof(LawyerBriefcase), 25),
			new LootPackItem(typeof(LeprechaunsLootChest), 25),
			new LootPackItem(typeof(LeprechaunsTrove), 25),
			new LootPackItem(typeof(LouisTreasuryChest), 25),
			new LootPackItem(typeof(MacingBonusChest), 25),
			new LootPackItem(typeof(MagesArcaneChest), 25),
			new LootPackItem(typeof(MagesRelicChest), 25),
			new LootPackItem(typeof(MaharajaTreasureChest), 25),
			new LootPackItem(typeof(MarioTreasureBox), 25),
			new LootPackItem(typeof(MedievalEnglandChest), 25),
			new LootPackItem(typeof(MerchantChest), 25),
			new LootPackItem(typeof(MerchantFortuneChest), 25),
			new LootPackItem(typeof(MermaidTreasureChest), 25),
			new LootPackItem(typeof(MillenniumTimeCapsule), 25),
			new LootPackItem(typeof(MimeSilentChest), 25),
			new LootPackItem(typeof(MirageChest), 25),
			new LootPackItem(typeof(ModMadnessTrunk), 25),
			new LootPackItem(typeof(MondainsDarkSecretsChest), 25),
			new LootPackItem(typeof(MysticalDaoChest), 25),
			new LootPackItem(typeof(MysticalEnchantersChest), 25),
			new LootPackItem(typeof(MysticEnigmaChest), 25),
			new LootPackItem(typeof(MysticGardenCache), 25),
			new LootPackItem(typeof(MysticMoonChest), 25),
			new LootPackItem(typeof(NaturesBountyChest), 25),
			new LootPackItem(typeof(NavyCaptainsChest), 25),
			new LootPackItem(typeof(NecroAlchemicalChest), 25),
			new LootPackItem(typeof(NeonNightsChest), 25),
			new LootPackItem(typeof(NeroChest), 25),
			new LootPackItem(typeof(NinjaChest), 25),
			new LootPackItem(typeof(NordicExplorersChest), 25),
			new LootPackItem(typeof(PatriotCache), 25),
			new LootPackItem(typeof(PeachRoyalCache), 25),
			new LootPackItem(typeof(PharaohsReliquary), 25),
			new LootPackItem(typeof(PharaohsTreasure), 25),
			new LootPackItem(typeof(PixieDustChest), 25),
			new LootPackItem(typeof(PokeballTreasureChest), 25),
			new LootPackItem(typeof(PolishRoyalChest), 25),
			new LootPackItem(typeof(PopStarsTrove), 25),
			new LootPackItem(typeof(RadBoomboxTrove), 25),
			new LootPackItem(typeof(Radical90sRelicsChest), 25),
			new LootPackItem(typeof(RadRidersStash), 25),
			new LootPackItem(typeof(RailwayWorkersChest), 25),
			new LootPackItem(typeof(RebelChest), 25),
			new LootPackItem(typeof(RenaissanceCollectorsChest), 25),
			new LootPackItem(typeof(RetroArcadeChest), 25),
			new LootPackItem(typeof(RevolutionaryChess), 25),
			new LootPackItem(typeof(RevolutionaryChest), 25),
			new LootPackItem(typeof(RevolutionaryRelicChest), 25),
			new LootPackItem(typeof(RevolutionChest), 25),
			new LootPackItem(typeof(RhineValleyChest), 25),
			new LootPackItem(typeof(RiverPiratesChest), 25),
			new LootPackItem(typeof(RiverRaftersChest), 25),
			new LootPackItem(typeof(RockersVault), 25),
			new LootPackItem(typeof(RockNBallVault), 25),
			new LootPackItem(typeof(RockNRallVault), 25),
			new LootPackItem(typeof(RockNRollVault), 25),
			new LootPackItem(typeof(RoguesHiddenChest), 25),
			new LootPackItem(typeof(RomanBritanniaChest), 25),
			new LootPackItem(typeof(RomanEmperorsVault), 25),
			new LootPackItem(typeof(SamuraiHonorChest), 25),
			new LootPackItem(typeof(SamuraiStash), 25),
			new LootPackItem(typeof(SandstormChest), 25),
			new LootPackItem(typeof(ScholarEnlightenmentChest), 25),
			new LootPackItem(typeof(SeaDogsChest), 25),
			new LootPackItem(typeof(ShinobiSecretsChest), 25),
			new LootPackItem(typeof(SilkRoadTreasuresChest), 25),
			new LootPackItem(typeof(SilverScreenChest), 25),
			new LootPackItem(typeof(SithsVault), 25),
			new LootPackItem(typeof(SlavicBrosChest), 25),
			new LootPackItem(typeof(SlavicLegendsChest), 25),
			new LootPackItem(typeof(SmugglersCache), 25),
			new LootPackItem(typeof(SocialMediaMavensChest), 25),
			new LootPackItem(typeof(SorceressSecretsChest), 25),
			new LootPackItem(typeof(SpaceRaceCache), 25),
			new LootPackItem(typeof(SpartanTreasureChest), 25),
			new LootPackItem(typeof(SpecialChivalryChest), 25),
			new LootPackItem(typeof(SpecialWoodenChestConstantine), 25),
			new LootPackItem(typeof(SpecialWoodenChestExplorerLegacy), 25),
			new LootPackItem(typeof(SpecialWoodenChestFrench), 25),
			new LootPackItem(typeof(SpecialWoodenChestHelios), 25),
			new LootPackItem(typeof(SpecialWoodenChestIvan), 25),
			new LootPackItem(typeof(SpecialWoodenChestOisin), 25),
			new LootPackItem(typeof(SpecialWoodenChestTomoe), 25),
			new LootPackItem(typeof(SpecialWoodenChestWashington), 25),
			new LootPackItem(typeof(StarfleetsVault), 25),
			new LootPackItem(typeof(SugarplumFairyChest), 25),
			new LootPackItem(typeof(SwingTimeChest), 25),
			new LootPackItem(typeof(SwordsmanshipBonusChest), 25),
			new LootPackItem(typeof(TacticsBonusChest), 25),
			new LootPackItem(typeof(TangDynastyChest), 25),
			new LootPackItem(typeof(TechnicolorTalesChest), 25),
			new LootPackItem(typeof(TechnophilesCache), 25),
			new LootPackItem(typeof(TeutonicRelicChest), 25),
			new LootPackItem(typeof(TeutonicTreasuresChest), 25),
			new LootPackItem(typeof(ThiefsHideawayStash), 25),
			new LootPackItem(typeof(ToxicologistsTrove), 25),
			new LootPackItem(typeof(TrailblazersTrove), 25),
			new LootPackItem(typeof(TravelerChest), 25),
			new LootPackItem(typeof(TreasureChestOfTheQinDynasty), 25),
			new LootPackItem(typeof(TreasureChestOfTheThreeKingdoms), 25),
			new LootPackItem(typeof(TsarsLegacyChest), 25),
			new LootPackItem(typeof(TsarsRoyalChest), 25),
			new LootPackItem(typeof(TsarsTreasureChest), 25),
			new LootPackItem(typeof(TudorDynastyChest), 25),
			new LootPackItem(typeof(UndergroundAnarchistsCache), 25),
			new LootPackItem(typeof(USSRRelicsChest), 25),
			new LootPackItem(typeof(VenetianMerchantsStash), 25),
			new LootPackItem(typeof(VHSAdventureCache), 25),
			new LootPackItem(typeof(VictorianEraChest), 25),
			new LootPackItem(typeof(VikingChest), 25),
			new LootPackItem(typeof(VintnersVault), 25),
			new LootPackItem(typeof(VinylVault), 25),
			new LootPackItem(typeof(VirtuesGuardianChest), 25),
			new LootPackItem(typeof(WarOf1812Vault), 25),
			new LootPackItem(typeof(WarringStatesChest), 25),
			new LootPackItem(typeof(WingedHusChest), 25),
			new LootPackItem(typeof(WingedHussarsChest), 25),
			new LootPackItem(typeof(WitchsBrewChest), 25),
			new LootPackItem(typeof(WorkersRevolutionChest), 25),
			new LootPackItem(typeof(WorldWarIIChest), 25),
			new LootPackItem(typeof(WWIIValorChest), 25),
			new LootPackItem(typeof(AncientWood), 100)
			
		};
		#endregion

		#region ML definitions
		public static readonly LootPack MlRich =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "4d50+450"),
					new LootPackEntry(false, AosMagicItemsRichType1, 100.00, 1, 3, 0, 75),
					new LootPackEntry(false, AosMagicItemsRichType1, 80.00, 1, 3, 0, 75),
					new LootPackEntry(false, AosMagicItemsRichType1, 60.00, 1, 5, 0, 100),
					new LootPackEntry(false, Instruments, 1.00, 1)
				});
		#endregion

		#region SE definitions
		public static readonly LootPack SePoor =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "2d10+20"), new LootPackEntry(false, AosMagicItemsPoor, 1.00, 1, 5, 0, 100),
					new LootPackEntry(false, Instruments, 0.02, 1),
					new LootPackEntry(false, NewFilthyRichItems, 1.00, "1") // Assuming you want 1 item

				});

		public static readonly LootPack SeMeager =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "4d10+40"),
					new LootPackEntry(false, AosMagicItemsMeagerType1, 20.40, 1, 2, 0, 50),
					new LootPackEntry(false, AosMagicItemsMeagerType2, 10.20, 1, 5, 0, 100),
					new LootPackEntry(false, Instruments, 0.10, 1),
					new LootPackEntry(false, NewFilthyRichItems, 2.00, "1") // Assuming you want 1 item

				});

		public static readonly LootPack SeAverage =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "8d10+100"),
					new LootPackEntry(false, AosMagicItemsAverageType1, 32.80, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsAverageType1, 32.80, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsAverageType2, 19.50, 1, 7, 50, 100),
					new LootPackEntry(false, Instruments, 0.40, 1),
					new LootPackEntry(false, NewFilthyRichItems, 3.00, "1") // Assuming you want 1 item
				});

		public static readonly LootPack SeRich =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "15d10+225"),
					new LootPackEntry(false, AosMagicItemsRichType1, 76.30, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsRichType1, 76.30, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsRichType2, 61.70, 1, 7, 50, 100),
					new LootPackEntry(false, Instruments, 1.00, 1),
					new LootPackEntry(false, NewFilthyRichItems, 5.00, "1") // Assuming you want 1 item
				});

		public static readonly LootPack SeFilthyRich =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "3d100+400"),
					new LootPackEntry(false, AosMagicItemsFilthyRichType1, 79.50, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsFilthyRichType1, 79.50, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsFilthyRichType2, 77.60, 1, 7, 50, 100),
					new LootPackEntry(false, Instruments, 2.00, 1),
					new LootPackEntry(false, NewFilthyRichItems, 10.00, "1") // Assuming you want 1 item
				});

		public static readonly LootPack SeUltraRich =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "6d100+600"),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, Instruments, 2.00, 1),
					new LootPackEntry(false, NewFilthyRichItems, 15.00, "2") // Assuming you want 1 item
				});

		public static readonly LootPack SeSuperBoss =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "10d100+800"),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, Instruments, 2.00, 1),
					new LootPackEntry(false, NewFilthyRichItems, 100.00, "3") // Assuming you want 1 item
				});
		#endregion

		#region AOS definitions
		public static readonly LootPack AosPoor =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "1d10+10"), new LootPackEntry(false, AosMagicItemsPoor, 0.02, 1, 5, 0, 90),
					new LootPackEntry(false, Instruments, 0.02, 1)
				});

		public static readonly LootPack AosMeager =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "3d10+20"),
					new LootPackEntry(false, AosMagicItemsMeagerType1, 1.00, 1, 2, 0, 10),
					new LootPackEntry(false, AosMagicItemsMeagerType2, 0.20, 1, 5, 0, 90),
					new LootPackEntry(false, Instruments, 0.10, 1)
				});

		public static readonly LootPack AosAverage =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "5d10+50"),
					new LootPackEntry(false, AosMagicItemsAverageType1, 5.00, 1, 4, 0, 20),
					new LootPackEntry(false, AosMagicItemsAverageType1, 2.00, 1, 3, 0, 50),
					new LootPackEntry(false, AosMagicItemsAverageType2, 0.50, 1, 5, 0, 90),
					new LootPackEntry(false, Instruments, 0.40, 1)
				});

		public static readonly LootPack AosRich =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "10d10+150"),
					new LootPackEntry(false, AosMagicItemsRichType1, 20.00, 1, 4, 0, 40),
					new LootPackEntry(false, AosMagicItemsRichType1, 10.00, 1, 5, 0, 60),
					new LootPackEntry(false, AosMagicItemsRichType2, 1.00, 1, 5, 0, 90), new LootPackEntry(false, Instruments, 1.00, 1)
				});

		public static readonly LootPack AosFilthyRich =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "2d100+200"),
					new LootPackEntry(false, AosMagicItemsFilthyRichType1, 33.00, 1, 4, 0, 50),
					new LootPackEntry(false, AosMagicItemsFilthyRichType1, 33.00, 1, 4, 0, 60),
					new LootPackEntry(false, AosMagicItemsFilthyRichType2, 20.00, 1, 5, 0, 75),
					new LootPackEntry(false, AosMagicItemsFilthyRichType2, 5.00, 1, 5, 0, 100),
					new LootPackEntry(false, Instruments, 2.00, 1)
				});

		public static readonly LootPack AosUltraRich =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "5d100+500"),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 5, 35, 100),
					new LootPackEntry(false, Instruments, 2.00, 1)
				});

		public static readonly LootPack AosSuperBoss =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "5d100+500"),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 7, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 5, 50, 100),
					new LootPackEntry(false, AosMagicItemsUltraRich, 100.00, 1, 5, 50, 100),
					new LootPackEntry(false, Instruments, 2.00, 1)
				});
		#endregion

		#region Pre-AOS definitions
		public static readonly LootPack OldPoor =
			new LootPack(new[] {new LootPackEntry(true, Gold, 100.00, "1d25"), new LootPackEntry(false, Instruments, 0.02, 1)});

		public static readonly LootPack OldMeager =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "5d10+25"), new LootPackEntry(false, Instruments, 0.10, 1),
					new LootPackEntry(false, OldMagicItems, 1.00, 1, 1, 0, 60),
					new LootPackEntry(false, OldMagicItems, 0.20, 1, 1, 10, 70)
				});

		public static readonly LootPack OldAverage =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "10d10+50"), new LootPackEntry(false, Instruments, 0.40, 1),
					new LootPackEntry(false, OldMagicItems, 5.00, 1, 1, 20, 80),
					new LootPackEntry(false, OldMagicItems, 2.00, 1, 1, 30, 90),
					new LootPackEntry(false, OldMagicItems, 0.50, 1, 1, 40, 100)
				});

		public static readonly LootPack OldRich =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "10d10+250"), new LootPackEntry(false, Instruments, 1.00, 1),
					new LootPackEntry(false, OldMagicItems, 20.00, 1, 1, 60, 100),
					new LootPackEntry(false, OldMagicItems, 10.00, 1, 1, 65, 100),
					new LootPackEntry(false, OldMagicItems, 1.00, 1, 1, 70, 100)
				});

		public static readonly LootPack OldFilthyRich =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "2d125+400"), new LootPackEntry(false, Instruments, 2.00, 1),
					new LootPackEntry(false, OldMagicItems, 33.00, 1, 1, 50, 100),
					new LootPackEntry(false, OldMagicItems, 33.00, 1, 1, 60, 100),
					new LootPackEntry(false, OldMagicItems, 20.00, 1, 1, 70, 100),
					new LootPackEntry(false, OldMagicItems, 5.00, 1, 1, 80, 100)
				});

		public static readonly LootPack OldUltraRich =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "5d100+500"), new LootPackEntry(false, Instruments, 2.00, 1),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 40, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 40, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 50, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 50, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 60, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 60, 100)
				});

		public static readonly LootPack OldSuperBoss =
			new LootPack(
				new[]
				{
					new LootPackEntry(true, Gold, 100.00, "5d100+500"), new LootPackEntry(false, Instruments, 2.00, 1),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 40, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 40, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 40, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 50, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 50, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 50, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 60, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 60, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 60, 100),
					new LootPackEntry(false, OldMagicItems, 100.00, 1, 1, 70, 100)
				});
		#endregion

		#region Generic accessors
		public static LootPack Poor { get { return Core.SE ? SePoor : Core.AOS ? AosPoor : OldPoor; } }
		public static LootPack Meager { get { return Core.SE ? SeMeager : Core.AOS ? AosMeager : OldMeager; } }
		public static LootPack Average { get { return Core.SE ? SeAverage : Core.AOS ? AosAverage : OldAverage; } }
		public static LootPack Rich { get { return Core.SE ? SeRich : Core.AOS ? AosRich : OldRich; } }
		public static LootPack FilthyRich { get { return Core.SE ? SeFilthyRich : Core.AOS ? AosFilthyRich : OldFilthyRich; } }
		public static LootPack UltraRich { get { return Core.SE ? SeUltraRich : Core.AOS ? AosUltraRich : OldUltraRich; } }
		public static LootPack SuperBoss { get { return Core.SE ? SeSuperBoss : Core.AOS ? AosSuperBoss : OldSuperBoss; } }
		#endregion

		public static readonly LootPack LowScrolls = new LootPack(new[] {new LootPackEntry(false, LowScrollItems, 100.00, 1)});

		public static readonly LootPack MedScrolls = new LootPack(new[] {new LootPackEntry(false, MedScrollItems, 100.00, 1)});

		public static readonly LootPack HighScrolls =
			new LootPack(new[] {new LootPackEntry(false, HighScrollItems, 100.00, 1)});

		public static readonly LootPack Gems = new LootPack(new[] {new LootPackEntry(false, GemItems, 100.00, 1)});

		public static readonly LootPack Potions = new LootPack(new[] {new LootPackEntry(false, PotionItems, 100.00, 1)});

		#region Mondain's Legacy
		public static readonly LootPackItem[] ParrotItem = new[] {new LootPackItem(typeof(ParrotItem), 1)};

		public static readonly LootPack Parrot = new LootPack(new[] {new LootPackEntry(false, ParrotItem, 10.00, 1)});
		#endregion
	}

	public class LootPackEntry
	{
		private LootPackDice m_Quantity;

		private int m_MaxProps, m_MinIntensity, m_MaxIntensity;

		private readonly bool m_AtSpawnTime;

		private LootPackItem[] m_Items;

		public int Chance { get; set; }

		public LootPackDice Quantity { get { return m_Quantity; } set { m_Quantity = value; } }

		public int MaxProps { get { return m_MaxProps; } set { m_MaxProps = value; } }

		public int MinIntensity { get { return m_MinIntensity; } set { m_MinIntensity = value; } }

		public int MaxIntensity { get { return m_MaxIntensity; } set { m_MaxIntensity = value; } }

		public LootPackItem[] Items { get { return m_Items; } set { m_Items = value; } }

		public static bool IsInTokuno(IEntity e)
		{
			if (e == null)
			{
				return false;
			}

            Region r = Region.Find(e.Location, e.Map);

			if (r.IsPartOf("Fan Dancer's Dojo"))
			{
				return true;
			}

			if (r.IsPartOf("Yomotsu Mines"))
			{
				return true;
			}

			return e.Map == Map.Tokuno;
		}

		#region Mondain's Legacy
		public static bool IsMondain(IEntity e)
		{
            if (e == null)
                return false;

			return MondainsLegacy.IsMLRegion(Region.Find(e.Location, e.Map));
		}
		#endregion

		#region Stygian Abyss
		public static bool IsStygian/*Abyss*/(IEntity e)
		{
            if (e == null)
                return false;

            return e.Map == Map.TerMur || (!IsInTokuno(e) && !IsMondain(e) && Utility.RandomBool());
		}
		#endregion

		public Item Construct(Mobile from, int luckChance, bool spawning)
		{
			if (m_AtSpawnTime != spawning)
			{
				return null;
			}

			int totalChance = 0;

			for (int i = 0; i < m_Items.Length; ++i)
			{
				totalChance += m_Items[i].Chance;
			}

			int rnd = Utility.Random(totalChance);

			for (int i = 0; i < m_Items.Length; ++i)
			{
				LootPackItem item = m_Items[i];

                if (rnd < item.Chance)
                {
                    return Mutate(from, luckChance, item.Construct(IsInTokuno(from), IsMondain(from), IsStygian(from)));
                }

				rnd -= item.Chance;
			}

			return null;
		}

		private int GetRandomOldBonus()
		{
			int rnd = Utility.RandomMinMax(m_MinIntensity, m_MaxIntensity);

			if (50 > rnd)
			{
				return 1;
			}
			else
			{
				rnd -= 50;
			}

			if (25 > rnd)
			{
				return 2;
			}
			else
			{
				rnd -= 25;
			}

			if (14 > rnd)
			{
				return 3;
			}
			else
			{
				rnd -= 14;
			}

			if (8 > rnd)
			{
				return 4;
			}

			return 5;
		}

		public Item Mutate(Mobile from, int luckChance, Item item)
		{
			if (item != null)
			{
				if (item is BaseWeapon && 1 > Utility.Random(100))
				{
					item.Delete();
					item = new FireHorn();
					return item;
				}

				if (item is BaseWeapon || item is BaseArmor || item is BaseJewel || item is BaseHat)
				{
					if (Core.AOS)
					{
                        // Try to generate a new random item based on the creature killed
                        if (Core.HS && RandomItemGenerator.Enabled && from is BaseCreature)
                        {
                            if (RandomItemGenerator.GenerateRandomItem(item, ((BaseCreature)from).LastKiller, (BaseCreature)from))
                                return item;
                        }

                        int bonusProps = GetBonusProperties();
						int min = m_MinIntensity;
						int max = m_MaxIntensity;

						if (bonusProps < m_MaxProps && LootPack.CheckLuck(luckChance))
						{
							++bonusProps;
						}

						int props = 1 + bonusProps;

						// Make sure we're not spawning items with 6 properties.
						if (props > m_MaxProps)
						{
							props = m_MaxProps;
						}

                        // Use the older style random generation
						if (item is BaseWeapon)
						{
							BaseRunicTool.ApplyAttributesTo((BaseWeapon)item, false, luckChance, props, m_MinIntensity, m_MaxIntensity);
						}
						else if (item is BaseArmor)
						{
							BaseRunicTool.ApplyAttributesTo((BaseArmor)item, false, luckChance, props, m_MinIntensity, m_MaxIntensity);
						}
						else if (item is BaseJewel)
						{
							BaseRunicTool.ApplyAttributesTo((BaseJewel)item, false, luckChance, props, m_MinIntensity, m_MaxIntensity);
						}
						else if (item is BaseHat)
						{
							BaseRunicTool.ApplyAttributesTo((BaseHat)item, false, luckChance, props, m_MinIntensity, m_MaxIntensity);
						}
					}
					else // not aos
					{
						if (item is BaseWeapon)
						{
							BaseWeapon weapon = (BaseWeapon)item;

							if (80 > Utility.Random(100))
							{
								weapon.AccuracyLevel = (WeaponAccuracyLevel)GetRandomOldBonus();
							}

							if (60 > Utility.Random(100))
							{
								weapon.DamageLevel = (WeaponDamageLevel)GetRandomOldBonus();
							}

							if (40 > Utility.Random(100))
							{
								weapon.DurabilityLevel = (WeaponDurabilityLevel)GetRandomOldBonus();
							}

							if (5 > Utility.Random(100))
							{
								weapon.Slayer = SlayerName.Silver;
							}

							if (from != null && weapon.AccuracyLevel == 0 && weapon.DamageLevel == 0 && weapon.DurabilityLevel == 0 &&
								weapon.Slayer == SlayerName.None && 5 > Utility.Random(100))
							{
								weapon.Slayer = SlayerGroup.GetLootSlayerType(from.GetType());
							}
						}
						else if (item is BaseArmor)
						{
							BaseArmor armor = (BaseArmor)item;

							if (80 > Utility.Random(100))
							{
								armor.ProtectionLevel = (ArmorProtectionLevel)GetRandomOldBonus();
							}

							if (40 > Utility.Random(100))
							{
								armor.Durability = (ArmorDurabilityLevel)GetRandomOldBonus();
							}
						}
					}
				}
				else if (item is BaseInstrument)
				{
					SlayerName slayer = SlayerName.None;

					if (Core.AOS)
					{
						slayer = BaseRunicTool.GetRandomSlayer();
					}
					else
					{
						slayer = SlayerGroup.GetLootSlayerType(from.GetType());
					}

					if (slayer == SlayerName.None)
					{
						item.Delete();
						return null;
					}

					BaseInstrument instr = (BaseInstrument)item;

					instr.Quality = ItemQuality.Normal;
					instr.Slayer = slayer;
				}

				if (item.Stackable)
				{
					item.Amount = m_Quantity.Roll();
				}
			}

			return item;
		}

		public LootPackEntry(bool atSpawnTime, LootPackItem[] items, double chance, string quantity)
			: this(atSpawnTime, items, chance, new LootPackDice(quantity), 0, 0, 0)
		{ }

		public LootPackEntry(bool atSpawnTime, LootPackItem[] items, double chance, int quantity)
			: this(atSpawnTime, items, chance, new LootPackDice(0, 0, quantity), 0, 0, 0)
		{ }

		public LootPackEntry(
			bool atSpawnTime,
			LootPackItem[] items,
			double chance,
			string quantity,
			int maxProps,
			int minIntensity,
			int maxIntensity)
			: this(atSpawnTime, items, chance, new LootPackDice(quantity), maxProps, minIntensity, maxIntensity)
		{ }

		public LootPackEntry(
			bool atSpawnTime, LootPackItem[] items, double chance, int quantity, int maxProps, int minIntensity, int maxIntensity)
			: this(atSpawnTime, items, chance, new LootPackDice(0, 0, quantity), maxProps, minIntensity, maxIntensity)
		{ }

		public LootPackEntry(
			bool atSpawnTime,
			LootPackItem[] items,
			double chance,
			LootPackDice quantity,
			int maxProps,
			int minIntensity,
			int maxIntensity)
		{
			m_AtSpawnTime = atSpawnTime;
			m_Items = items;
			Chance = (int)(100 * chance);
			m_Quantity = quantity;
			m_MaxProps = maxProps;
			m_MinIntensity = minIntensity;
			m_MaxIntensity = maxIntensity;
		}

		public int GetBonusProperties()
		{
			int p0 = 0, p1 = 0, p2 = 0, p3 = 0, p4 = 0, p5 = 0;

			switch (m_MaxProps)
			{
				case 1:
					p0 = 3;
					p1 = 1;
					break;
				case 2:
					p0 = 6;
					p1 = 3;
					p2 = 1;
					break;
				case 3:
					p0 = 10;
					p1 = 6;
					p2 = 3;
					p3 = 1;
					break;
				case 4:
					p0 = 16;
					p1 = 12;
					p2 = 6;
					p3 = 5;
					p4 = 1;
					break;
				case 5:
					p0 = 30;
					p1 = 25;
					p2 = 20;
					p3 = 15;
					p4 = 9;
					p5 = 1;
					break;
			}

			int pc = p0 + p1 + p2 + p3 + p4 + p5;

			int rnd = Utility.Random(pc);

			if (rnd < p5)
			{
				return 5;
			}
			else
			{
				rnd -= p5;
			}

			if (rnd < p4)
			{
				return 4;
			}
			else
			{
				rnd -= p4;
			}

			if (rnd < p3)
			{
				return 3;
			}
			else
			{
				rnd -= p3;
			}

			if (rnd < p2)
			{
				return 2;
			}
			else
			{
				rnd -= p2;
			}

			if (rnd < p1)
			{
				return 1;
			}

			return 0;
		}
	}

	public class LootPackItem
	{
		private Type m_Type;

		public Type Type { get { return m_Type; } set { m_Type = value; } }

		public int Chance { get; set; }

		private static readonly Type[] m_BlankTypes = new[] {typeof(BlankScroll)};

		private static readonly Type[][] m_NecroTypes = new[]
		{
			new[] // low
			{
				typeof(AnimateDeadScroll), typeof(BloodOathScroll), typeof(CorpseSkinScroll), typeof(CurseWeaponScroll),
				typeof(EvilOmenScroll), typeof(HorrificBeastScroll), typeof(MindRotScroll), typeof(PainSpikeScroll),
				typeof(SummonFamiliarScroll), typeof(WraithFormScroll)
			},
			new[] // med
			{typeof(LichFormScroll), typeof(PoisonStrikeScroll), typeof(StrangleScroll), typeof(WitherScroll)},
			((Core.SE)
				 ? new[] // high
				 {typeof(VengefulSpiritScroll), typeof(VampiricEmbraceScroll), typeof(ExorcismScroll)}
				 : new[] // high
				 {typeof(VengefulSpiritScroll), typeof(VampiricEmbraceScroll)})
		};

        private static readonly SpellbookType[] m_BookTypes = new[]
        {
            SpellbookType.Regular, SpellbookType.Necromancer, SpellbookType.Mystic
        };

        private static readonly int[][] m_ScrollIndexMin = new[]
        {
            new[] {0, 8, 16, 24, 32, 40, 48, 56},
            new[] {0, 2, 4, 6, 8, 10, 12, 14},
            new[] {0, 2, 4, 6, 8, 10, 12, 14},
        };

        private static readonly int[][] m_ScrollIndexMax = new[]
        {
            new[] {7, 15, 23, 31, 39, 47, 55, 63},
            new[] {1, 3, 5, 7, 9, 11, 13, 14},
            new[] {1, 3, 5, 7, 9, 11, 13, 14},
        };

        public static Item RandomScroll(int minCircle, int maxCircle)
		{
			--minCircle;
			--maxCircle;

            int minIndex, maxIndex, rnd, rndMax;
            SpellbookType spellBookType;

            // Magery scrolls are weighted at 4 because there are four times as many magery
            // spells as other scolls of magic
            rndMax = 4;
            if (Core.ML)
                rndMax += 2;
            else if (Core.AOS)
                rndMax += 1;
            rnd = Utility.Random(rndMax);
            rnd -= 3;
            if (rnd < 0)
                rnd = 0;

            minIndex = m_ScrollIndexMin[rnd][minCircle];
            maxIndex = m_ScrollIndexMax[rnd][maxCircle];
            if (rnd == 2 && maxCircle == 7)
                ++maxIndex;
            spellBookType = m_BookTypes[rnd];

            return Loot.RandomScroll(minIndex, maxIndex, spellBookType);
		}

		public Item Construct(bool inTokuno, bool isMondain, bool isStygian)
		{
			try
			{
				Item item;

				if (m_Type == typeof(BaseRanged))
				{
					item = Loot.RandomRangedWeapon(inTokuno, isMondain, isStygian );
				}
				else if (m_Type == typeof(BaseWeapon))
				{
					item = Loot.RandomWeapon(inTokuno, isMondain, isStygian );
				}
				else if (m_Type == typeof(BaseArmor))
				{
					item = Loot.RandomArmorOrHat(inTokuno, isMondain, isStygian);
				}
				else if (m_Type == typeof(BaseShield))
				{
					item = Loot.RandomShield(isStygian);
				}
				else if (m_Type == typeof(BaseJewel))
				{
					item = Core.AOS ? Loot.RandomJewelry(isStygian) : Loot.RandomArmorOrShieldOrWeapon(isStygian);
				}
				else if (m_Type == typeof(BaseInstrument))
				{
					item = Loot.RandomInstrument();
				}
				else if (m_Type == typeof(Amber)) // gem
				{
					item = Loot.RandomGem();
				}
				else if (m_Type == typeof(ClumsyScroll)) // low scroll
				{
					item = RandomScroll(1, 3);
				}
				else if (m_Type == typeof(ArchCureScroll)) // med scroll
				{
					item = RandomScroll(4, 7);
				}
				else if (m_Type == typeof(SummonAirElementalScroll)) // high scroll
				{
					item = RandomScroll(8, 8);
				}
				else
				{
					item = Activator.CreateInstance(m_Type) as Item;
				}

				return item;
			}
			catch
			{ }

			return null;
		}

		public LootPackItem(Type type, int chance)
		{
			m_Type = type;
			Chance = chance;
		}
	}

	public class LootPackDice
	{
		private int m_Count, m_Sides, m_Bonus;

		public int Count { get { return m_Count; } set { m_Count = value; } }

		public int Sides { get { return m_Sides; } set { m_Sides = value; } }

		public int Bonus { get { return m_Bonus; } set { m_Bonus = value; } }

		public int Roll()
		{
			int v = m_Bonus;

			for (int i = 0; i < m_Count; ++i)
			{
				v += Utility.Random(1, m_Sides);
			}

			return v;
		}

		public LootPackDice(string str)
		{
			int start = 0;
			int index = str.IndexOf('d', start);

			if (index < start)
			{
				return;
			}

			m_Count = Utility.ToInt32(str.Substring(start, index - start));

			bool negative;

			start = index + 1;
			index = str.IndexOf('+', start);

			if (negative = (index < start))
			{
				index = str.IndexOf('-', start);
			}

			if (index < start)
			{
				index = str.Length;
			}

			m_Sides = Utility.ToInt32(str.Substring(start, index - start));

			if (index == str.Length)
			{
				return;
			}

			start = index + 1;
			index = str.Length;

			m_Bonus = Utility.ToInt32(str.Substring(start, index - start));

			if (negative)
			{
				m_Bonus *= -1;
			}
		}

		public LootPackDice(int count, int sides, int bonus)
		{
			m_Count = count;
			m_Sides = sides;
			m_Bonus = bonus;
		}
	}
}