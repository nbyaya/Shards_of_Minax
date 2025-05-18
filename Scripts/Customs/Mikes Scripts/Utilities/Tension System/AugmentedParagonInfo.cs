using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Bittiez.CustomSystems; // for TensionManager
using System.Collections.Generic;
using Server.Engines.XmlSpawner2; // For XmlAttach and XML attachments

namespace Server.Mobiles
{
    // Basic container for each augmented paragon's stats and unique XML ability.
    public class AugmentedParagonInfo
    {
        public string Name { get; set; }
        public int[] HueRange { get; set; } // Supports multiple hues
        public bool UseRandomHue { get; set; } // If true, uses a completely random hue

        // Base buff values – these will be scaled based on Tension.
        public double BaseHitsBuff { get; set; }
        public double BaseStrBuff { get; set; }
        public double BaseIntBuff { get; set; }
        public double BaseDexBuff { get; set; }
        public double BaseSkillsBuff { get; set; }
        public double BaseSpeedBuff { get; set; }
        public double BaseFameBuff { get; set; }
        public double BaseKarmaBuff { get; set; }
        public int BaseDamageBuff { get; set; }

        // The XML ability type to attach.
        public Type AbilityType { get; set; }

        public AugmentedParagonInfo(
            string name,
            int[] hueRange,
            bool useRandomHue,
            double baseHitsBuff,
            double baseStrBuff,
            double baseIntBuff,
            double baseDexBuff,
            double baseSkillsBuff,
            double baseSpeedBuff,
            double baseFameBuff,
            double baseKarmaBuff,
            int baseDamageBuff,
            Type abilityType)
        {
            Name = name;
            HueRange = hueRange;
            UseRandomHue = useRandomHue;
            BaseHitsBuff = baseHitsBuff;
            BaseStrBuff = baseStrBuff;
            BaseIntBuff = baseIntBuff;
            BaseDexBuff = baseDexBuff;
            BaseSkillsBuff = baseSkillsBuff;
            BaseSpeedBuff = baseSpeedBuff;
            BaseFameBuff = baseFameBuff;
            BaseKarmaBuff = baseKarmaBuff;
            BaseDamageBuff = baseDamageBuff;
            AbilityType = abilityType;
        }

        /// <summary>
        /// Returns a multiplier for a given buff based on the current tension.
        /// Example: 5000 tension gives a +100% bonus.
        /// </summary>
        public double GetBuffMultiplier(double baseValue)
        {
            double tension = TensionManager.Tension;
            double scaleFactor = 1.0 + (tension / 50000.0);
            return baseValue * scaleFactor;
        }
    }

    // ExtendedParagon picks from multiple paragon templates (each with its own XML ability).
    public static class ExtendedParagon
    {
        public static List<AugmentedParagonInfo> AllParagonTypes = new List<AugmentedParagonInfo>();

        static ExtendedParagon()
        {
            // -- Predefined Paragon Types with Assigned XML Abilities --
            // These are your “classic” paragon templates.
            AllParagonTypes.Add(new AugmentedParagonInfo(
                "Stone",
                new int[] { 0x0 },
                false,
                5.0, 1.05, 1.20, 1.20, 1.20, 1.20, 1.40, 1.40, 5,
                typeof(XmlGraniteSlam)          // (e.g., a crushing, earth-shattering slam)
            ));
            AllParagonTypes.Add(new AugmentedParagonInfo(
                "Abomination",
                new int[] { 0x4001 },
                false,
                5.0, 1.10, 1.25, 1.15, 1.25, 1.20, 1.40, 1.40, 6,
                typeof(XmlChaoticSurge)          // (a surge of chaotic energy)
            ));
            AllParagonTypes.Add(new AugmentedParagonInfo(
                "Ancient",
                new int[] { 0x845 },
                false,
                5.0, 1.15, 1.15, 1.15, 1.15, 1.25, 1.40, 1.40, 5,
                typeof(XmlFossilBurst)           // (an ancient burst echoing from the past)
            ));
            AllParagonTypes.Add(new AugmentedParagonInfo(
                "Rainbow",
                new int[] { },
                true,  // Uses a completely random hue (rainbow effect)
                6.0, 1.20, 1.25, 1.25, 1.20, 1.30, 1.50, 1.50, 7,
                typeof(XmlPrismaticBurst)        // (a prismatic, color-shifting burst)
            ));
            AllParagonTypes.Add(new AugmentedParagonInfo(
                "Thunderous",
                new int[] { 0x7DA, 0x81F, 0xB77 },
                false,
                6.0, 1.10, 1.30, 1.10, 1.25, 1.50, 1.60, 1.60, 7,
                typeof(XmlLightningStrike)       // (a strike of pure lightning)
            ));
            AllParagonTypes.Add(new AugmentedParagonInfo(
                "Frozen Horror",
                new int[] { 0x480, 0x481 },
                false,
                7.0, 1.15, 1.30, 1.10, 1.20, 1.30, 1.50, 1.50, 6,
                typeof(XmlFrostNova)             // (an explosion of freezing power)
            ));
            AllParagonTypes.Add(new AugmentedParagonInfo(
                "Infernal",
                new int[] { 0x489, 0x48A },
                false,
                6.5, 1.30, 1.10, 1.30, 1.10, 1.25, 1.60, 1.60, 8,
                typeof(XmlHellfireStorm)         // (a raging inferno)
            ));
            AllParagonTypes.Add(new AugmentedParagonInfo(
                "Undying",
                new int[] { 0x4001, 0x83E },
                false,
                7.5, 1.15, 1.20, 1.10, 1.30, 1.25, 1.55, 1.55, 7,
                typeof(XmlPhantomStrike)         // (a spectral, relentless assault)
            ));
            AllParagonTypes.Add(new AugmentedParagonInfo(
                "Temporal",
                new int[] { 0x59B, 0x7DA },
                false,
                5.0, 1.10, 1.25, 1.25, 1.50, 1.75, 1.30, 1.30, 5,
                typeof(XmlCycloneRampage)        // (a whirlwind of time-bending force)
            ));

            // -- Generate Unique Paragon Types for the Remaining XML Abilities --
            // These are created from the remaining list (excluding the nine above).
            string[] remainingAbilities = new string[]
            {
                "XmlAbyssalWave",
                "XmlAirborneEscape",
                "XmlBackStrike",
                "XmlBananaBomb",
                "XmlBlazingCharge",
                "XmlBlazingTrail",
                "XmlBoilingSurge",
                "XmlBreezeHeal",
                "XmlBubbleBurst",
                "XmlBubbleShield",
                "XmlCrescendoOfJoy",
                "XmlCycloneCharge",
                "XmlDisguise",
                "XmlDreamWeave",
                "XmlDreamyAura",
                "XmlEarthquake",
                "XmlElectricalSurge",
                "XmlEruption",
                "XmlEvasiveManeuver",
                "XmlFieryIllusion",
                "XmlFireBreathAttack",
                "XmlFireWalk",
                "XmlFrenziedAttack",
                "XmlFrostBreath",
                "XmlFrostyTrail",
                "XmlGaleStrike",
                "XmlGlitterShield",
                "XmlGoldRain",
                "XmlGravityWarp",
                "XmlGroundSlam",
                "XmlGustBarrier",
                "XmlHarmonyEcho",
                "XmlHeavenlyStrike",
                "XmlIllusionAbility",
                "XmlInvisibility",
                "XmlLavaFlow",
                "XmlLavaWave",
                "XmlMelodyOfPeace",
                "XmlMistyStep",
                "XmlMoltenBlast",
                "XmlMudBomb",
                "XmlMudTrap",
                "XmlNuke",
                "XmlPhantomBurn",
                // "XmlPrismaticBurst", // Already used for Rainbow
                "XmlRadiantShield",
                "XmlRage",
                "XmlRainOfWrath",
                "XmlRallyingRoar",
                "XmlRandomAbility",
                "XmlRandomMinionStrike",
                "XmlResonantAura",
                "XmlSavageStrike",
                "XmlScorchedBite",
                "XmlSilentGale",
                "XmlSolarBurst",
                "XmlSolarFlare",
                "XmlSoothingWind",
                "XmlSparkleBlast",
                "XmlSparklingAura",
                "XmlStardustBeam",
                "XmlStarlightBurst",
                "XmlStarShower",
                "XmlStaticShock",
                "XmlStatusEffectAttachment",
                "XmlStormDash",
                "XmlSunlitHeal",
                "XmlTeleport",
                "XmlTeleportAbility",
                "XmlTempestBreath",
                "XmlTidalPull",
                "XmlTideSurge",
                "XmlToxicSludge",
                "XmlTrap",
                "XmlTrickDecoy",
                "XmlTrickstersGambit",
                "XmlTrueFear",
                "XmlVolcanicEruption",
                "XmlVortexPull",
                "XmlWaterVortex",
                "XmlWhirlwindFury",
                "XmlAxeBreath",
                "XmlAxeCircle",
                "XmlAxeLine",
                "XmlBackstab",
                "XmlBeeBreath",
                "XmlBeeCircle",
                "XmlBeeLine",
                "XmlBladesBreath",
                "XmlBladesCircle",
                "XmlBladesLine",
                "XmlBoulderBreath",
                "XmlBoulderCircle",
                "XmlBoulderLine",
                "XmlBreathAttack",
                "XmlChillTouch",
                "XmlCircleFireAttack",
                "XmlCrankBreath",
                "XmlCrankCircle",
                "XmlCrankLine",
                "XmlCurtainBreath",
                "XmlCurtainCircle",
                "XmlCurtainLine",
                "XmlDeerBreath",
                "XmlDeerCircle",
                "XmlDeerLine",
                "XmlDevour",
                "XmlDisarm",
                "XmlDisarmor",
                "XmlDVortexBreath",
                "XmlDVortexCircle",
                "XmlDVortexLine",
                "XmlEnrage",
                "XmlFlaskBreath",
                "XmlFlaskCircle",
                "XmlFlaskLine",
                "XmlFlesheater",
                "XmlFreezeStrike",
                "XmlFrenzy",
                "XmlFTreeBreath",
                "XmlFTreeCircle",
                "XmlFTreeLine",
                "XmlGasBreath",
                "XmlGasCircle",
                "XmlGasLine",
                "XmlGateBreath",
                "XmlGateCircle",
                "XmlGateLine",
                "XmlGlowBreath",
                "XmlGlowCircle",
                "XmlGlowLine",
                "XmlGrasp",
                "XmlGuillotineBreath",
                "XmlGuillotineCircle",
                "XmlGuillotineLine",
                "XmlHeadBreath",
                "XmlHeadCircle",
                "XmlHeadLine",
                "XmlHeartBreath",
                "XmlHeartCircle",
                "XmlHeartLine",
                "XmlLineAttack",
                "XmlMagmaThrow",
                "XmlMaidenBreath",
                "XmlMaidenCircle",
                "XmlMaidenLine",
                "XmlMushroomBreath",
                "XmlMushroomCircle",
                "XmlMushroomLine",
                "XmlNutcrackerBreath",
                "XmlNutcrackerCircle",
                "XmlNutcrackerLine",
                "XmlOFlaskBreath",
                "XmlOFlaskCircle",
                "XmlOFlaskLine",
                "XmlParaBreath",
                "XmlParaCircle",
                "XmlParaLine",
                "XmlPoisonAppleThrow",
                "XmlPrismaticSpray",
                "XmlRuneBreath",
                "XmlRuneCircle",
                "XmlRuneLine",
                "XmlSawBreath",
                "XmlSawCircle",
                "XmlSawLine",
                "XmlSkeletonBreath",
                "XmlSkeletonCircle",
                "XmlSkeletonLine",
                "XmlSkullBreath",
                "XmlSkullCircle",
                "XmlSkullLine",
                "XmlSmokeBreath",
                "XmlSmokeCircle",
                "XmlSmokeLine",
                "XmlSparkleBreath",
                "XmlSparkleCircle",
                "XmlSparkleLine",
                "XmlSpikeBreath",
                "XmlSpikeCircle",
                "XmlSpikeLine",
                "XmlSpineBarrage",
                "XmlSting",
                "XmlStoneBreath",
                "XmlStoneCircle",
                "XmlStoneLine",
                "XmlTheftStrike",
                "XmlTimeBreath",
                "XmlTimeCircle",
                "XmlTimeLine",
                "XmlTrapBreath",
                "XmlTrapCircle",
                "XmlTrapLine",
                "XmlVortexBreath",
                "XmlVortexCircle",
                "XmlVortexLine",
                "XmlWaterBreath",
                "XmlWaterCircle",
                "XmlWaterLine",
                "XmlWeaken",
                "XmlWebCooldown",				
                "XmlWindBlast"
            };

            // For each remaining ability, generate a unique paragon template.
            // Instead of using a fixed hue, calculate an individual hue from the ability name.
            foreach (string abilityName in remainingAbilities)
            {
                // Generate a paragon name by removing the "Xml" prefix and adding spaces before capitals.
                string paragonName = GenerateParagonName(abilityName);

                // Calculate a unique hue from the ability name's hash code.
                int uniqueHue = Math.Abs(abilityName.GetHashCode()) % 3000;
                if (uniqueHue == 0)
                    uniqueHue = 1;
                int[] hueRange = new int[] { uniqueHue };
                bool useRandomHue = false;

                // Generic buff values for these unique paragon types.
                double baseHitsBuff = 5.0;
                double baseStrBuff = 1.10;
                double baseIntBuff = 1.10;
                double baseDexBuff = 1.10;
                double baseSkillsBuff = 1.15;
                double baseSpeedBuff = 1.20;
                double baseFameBuff = 1.30;
                double baseKarmaBuff = 1.30;
                int baseDamageBuff = 5;

                // Attempt to get the Type of the XML ability from the known namespace.
                Type abilityType = Type.GetType("Server.Engines.XmlSpawner2." + abilityName, false, true);
                if (abilityType == null)
                    continue; // Skip if not found

                AllParagonTypes.Add(new AugmentedParagonInfo(
                    paragonName,
                    hueRange,
                    useRandomHue,
                    baseHitsBuff,
                    baseStrBuff,
                    baseIntBuff,
                    baseDexBuff,
                    baseSkillsBuff,
                    baseSpeedBuff,
                    baseFameBuff,
                    baseKarmaBuff,
                    baseDamageBuff,
                    abilityType
                ));
            }
        }

        // Helper method to generate a paragon name from an XML ability name.
        private static string GenerateParagonName(string abilityName)
        {
            // Remove "Xml" prefix if present.
            if (abilityName.StartsWith("Xml"))
                abilityName = abilityName.Substring(3);
            // Insert spaces before capital letters.
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in abilityName)
            {
                if (char.IsUpper(c) && sb.Length > 0)
                    sb.Append(" ");
                sb.Append(c);
            }
            return sb.ToString().Trim();
        }

        /// <summary>
        /// Converts a BaseCreature into a randomly chosen augmented paragon type.
        /// Applies tension-scaled stat buffs, assigns hue, and attaches the appropriate XML ability.
        /// </summary>
        public static void ConvertToAugmentedParagon(BaseCreature bc)
        {
            if (bc.IsParagon || !bc.CanBeParagon)
                return;

            var info = AllParagonTypes[Utility.Random(AllParagonTypes.Count)];

            bc.ParagonName = info.Name;
            bc.IsParagon = true;

            // Set the hue: either from the fixed range or a random one if flagged.
            if (info.UseRandomHue)
                bc.Hue = Utility.RandomMinMax(1, 3000);
            else
                bc.Hue = info.HueRange[Utility.Random(info.HueRange.Length)];

            // Get tension-based scaling factors.
            double hitsBuff = info.GetBuffMultiplier(info.BaseHitsBuff);
            double strBuff = info.GetBuffMultiplier(info.BaseStrBuff);
            double intBuff = info.GetBuffMultiplier(info.BaseIntBuff);
            double dexBuff = info.GetBuffMultiplier(info.BaseDexBuff);
            double skillsBuff = info.GetBuffMultiplier(info.BaseSkillsBuff);
            double speedBuff = info.GetBuffMultiplier(info.BaseSpeedBuff);
            double fameBuff = info.GetBuffMultiplier(info.BaseFameBuff);
            double karmaBuff = info.GetBuffMultiplier(info.BaseKarmaBuff);
            int damageBuff = (int)info.GetBuffMultiplier(info.BaseDamageBuff);

            // Apply stat buffs.
            if (bc.HitsMaxSeed >= 0)
                bc.HitsMaxSeed = (int)(bc.HitsMaxSeed * hitsBuff);
            bc.RawStr = (int)(bc.RawStr * strBuff);
            bc.RawInt = (int)(bc.RawInt * intBuff);
            bc.RawDex = (int)(bc.RawDex * dexBuff);

            bc.Hits = bc.HitsMax;
            bc.Mana = bc.ManaMax;
            bc.Stam = bc.StamMax;

            for (int i = 0; i < bc.Skills.Length; i++)
            {
                Skill skill = (Skill)bc.Skills[i];
                if (skill.Base > 0.0)
                    skill.Base *= skillsBuff;
            }

            bc.PassiveSpeed /= speedBuff;
            bc.ActiveSpeed /= speedBuff;
            bc.CurrentSpeed = bc.PassiveSpeed;

            bc.DamageMin += damageBuff;
            bc.DamageMax += damageBuff;

            if (bc.Fame > 0)
                bc.Fame = (int)(bc.Fame * fameBuff);
            if (bc.Karma != 0)
                bc.Karma = (int)(bc.Karma * karmaBuff);

            // Attach the specific XML ability using the stored AbilityType.
			if (info.AbilityType != null)
			{
				try
				{
					// Ensure the ability has a parameterless constructor before creating an instance
					if (info.AbilityType.GetConstructor(Type.EmptyTypes) != null)
					{
						object abilityInstance = Activator.CreateInstance(info.AbilityType);

						// Explicitly cast the object to XmlAttachment before attaching
						if (abilityInstance is XmlAttachment attachment)
						{
							XmlAttach.AttachTo(bc, attachment);
						}
						else
						{
							Console.WriteLine($"Warning: {info.AbilityType.Name} is not a valid XmlAttachment, skipping.");
						}
					}
					else
					{
						Console.WriteLine($"Warning: {info.AbilityType.Name} has no parameterless constructor, skipping.");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error attaching {info.AbilityType.Name} to {bc.Name}: {ex.Message}");
				}
			}

        }
    }
}
