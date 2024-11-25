using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlRandomAbility : XmlAttachment
    {
        private int m_MinAbilities;
        private int m_MaxAbilities;

        // Properties to allow modification via commands or scripts
        [CommandProperty(AccessLevel.GameMaster)]
        public int MinAbilities { get { return m_MinAbilities; } set { m_MinAbilities = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxAbilities { get { return m_MaxAbilities; } set { m_MaxAbilities = value; } }

        // List of available abilities, include those that might require parameters
        public static readonly List<Ability> AvailableAbilities = new List<Ability>
        {
            new Ability(typeof(XmlEarthquakeStrike), 20, 0), // Requires two integer parameters
            new Ability(typeof(XmlFrostStrike)), // No parameters
            new Ability(typeof(XmlMinionStrike), "AirElemental"), // Requires a string parameter
            new Ability(typeof(XmlEarthquakeStrike)),
            new Ability(typeof(XmlTheftStrike)),
            new Ability(typeof(XmlFrostStrike)),
            new Ability(typeof(XmlFTreeBreath)),
            new Ability(typeof(XmlHeartBreath)),
            new Ability(typeof(XmlNutcrackerBreath)),
            new Ability(typeof(XmlDeerBreath)),
            new Ability(typeof(XmlParaBreath)),
            new Ability(typeof(XmlGateBreath)),
            new Ability(typeof(XmlTrapBreath)),
            new Ability(typeof(XmlOFlaskBreath)),
            new Ability(typeof(XmlSkeletonBreath)),
            new Ability(typeof(XmlSkullCircle)),
            new Ability(typeof(XmlDVortexCircle)),
            new Ability(typeof(XmlGlowCircle)),
            new Ability(typeof(XmlBladesCircle)),
            new Ability(typeof(XmlVortexCircle)),
            new Ability(typeof(XmlSparkleCircle)),
            new Ability(typeof(XmlSmokeCircle)),
            new Ability(typeof(XmlBoulderCircle)),
            new Ability(typeof(XmlPoisonAppleThrow)),
            new Ability(typeof(XmlMagmaThrow)),
            new Ability(typeof(XmlBreathAttack)),
            new Ability(typeof(XmlCircleFireAttack)),
            new Ability(typeof(XmlLineAttack)),
            new Ability(typeof(XmlManaBurn)),
            new Ability(typeof(XmlArcaneExplosion)),
            new Ability(typeof(XmlSilenceStrike)),
            new Ability(typeof(XmlPoisonCloud)),
            new Ability(typeof(XmlFreezeStrike)),
            new Ability(typeof(XmlBlazeStrike)),
            new Ability(typeof(XmlWhirlwind)),
            new Ability(typeof(XmlGrasp)),
            new Ability(typeof(XmlBackstab)),
            new Ability(typeof(XmlChillTouch)),
            new Ability(typeof(XmlDevour)),
            new Ability(typeof(XmlFlesheater)),
            new Ability(typeof(XmlSting)),
            new Ability(typeof(XmlEnrage)),
            new Ability(typeof(XmlWeaken)),
            new Ability(typeof(XmlFrenzy)),
            new Ability(typeof(XmlWebCooldown)),
            new Ability(typeof(XmlAbyssalWave)),
            new Ability(typeof(XmlAirborneEscape)),
            new Ability(typeof(XmlBackStrike)),
            new Ability(typeof(XmlBananaBomb)),
            new Ability(typeof(XmlBlazingCharge)),
            new Ability(typeof(XmlBlazingTrail)),
            new Ability(typeof(XmlBoilingSurge)),
            new Ability(typeof(XmlBreezeHeal)),
            new Ability(typeof(XmlBubbleBurst)),
            new Ability(typeof(XmlBubbleShield)),
            new Ability(typeof(XmlChaoticSurge)),
            new Ability(typeof(XmlCosmicCloak)),
            new Ability(typeof(XmlCrescendoOfJoy)),
            new Ability(typeof(XmlCycloneCharge)),
            new Ability(typeof(XmlCycloneRampage)),
            new Ability(typeof(XmlDisguise)),
            new Ability(typeof(XmlDreamDust)),
            new Ability(typeof(XmlDreamWeave)),
            new Ability(typeof(XmlDreamyAura)),
            new Ability(typeof(XmlEarthquake)),
            new Ability(typeof(XmlElectricalSurge)),
            new Ability(typeof(XmlEruption)),
            new Ability(typeof(XmlEvasiveManeuver)),
            new Ability(typeof(XmlFieryIllusion)),
            new Ability(typeof(XmlFireBreathAttack)),
            new Ability(typeof(XmlFireWalk)),
            new Ability(typeof(XmlFlameCoil)),
            new Ability(typeof(XmlFossilBurst)),
            new Ability(typeof(XmlFrenziedAttack)),
            new Ability(typeof(XmlFrostBreath)),
            new Ability(typeof(XmlFrostNova)),
            new Ability(typeof(XmlFrostyTrail)),
            new Ability(typeof(XmlGaleStrike)),
            new Ability(typeof(XmlGlitterShield)),
            new Ability(typeof(XmlGoldRain)),
            new Ability(typeof(XmlGraniteSlam)),
            new Ability(typeof(XmlGravityWarp)),
            new Ability(typeof(XmlGroundSlam)),
            new Ability(typeof(XmlGustBarrier)),
            new Ability(typeof(XmlHarmonyEcho)),
            new Ability(typeof(XmlHeavenlyStrike)),
            new Ability(typeof(XmlHellfireStorm)),
            new Ability(typeof(XmlIllusionAbility)),
            new Ability(typeof(XmlInfernoAura)),
            new Ability(typeof(XmlInvisibility)),
            new Ability(typeof(XmlLavaFlow)),
            new Ability(typeof(XmlLavaWave)),
            new Ability(typeof(XmlLightningStrike)),
            new Ability(typeof(XmlMelodyOfPeace)),
            new Ability(typeof(XmlMistyStep)),
            new Ability(typeof(XmlMoltenBlast)),
            new Ability(typeof(XmlMudBomb)),
            new Ability(typeof(XmlMudTrap)),
            new Ability(typeof(XmlNuke)),
            new Ability(typeof(XmlPhantomBurn)),
            new Ability(typeof(XmlPhantomStrike)),
            new Ability(typeof(XmlPrismaticBurst)),
            new Ability(typeof(XmlRadiantShield)),
            new Ability(typeof(XmlRage)),
            new Ability(typeof(XmlRainOfWrath)),
            new Ability(typeof(XmlRallyingRoar)),
            new Ability(typeof(XmlResonantAura)),
            new Ability(typeof(XmlSavageStrike)),
            new Ability(typeof(XmlScorchedBite)),
            new Ability(typeof(XmlSilentGale)),
            new Ability(typeof(XmlSolarBurst)),
            new Ability(typeof(XmlSolarFlare)),
            new Ability(typeof(XmlSoothingWind)),
            new Ability(typeof(XmlSparkleBlast)),
            new Ability(typeof(XmlSparklingAura)),
            new Ability(typeof(XmlStardustBeam)),
            new Ability(typeof(XmlStarlightBurst)),
            new Ability(typeof(XmlStarShower)),
            new Ability(typeof(XmlStaticShock)),
            new Ability(typeof(XmlStatusEffectAttachment)),
            new Ability(typeof(XmlStormDash)),
            new Ability(typeof(XmlSunlitHeal)),
            new Ability(typeof(XmlTeleport)),
            new Ability(typeof(XmlTeleportAbility)),
            new Ability(typeof(XmlTempestBreath)),
            new Ability(typeof(XmlTidalPull)),
            new Ability(typeof(XmlTideSurge)),
            new Ability(typeof(XmlToxicSludge)),
            new Ability(typeof(XmlTrap)),
            new Ability(typeof(XmlTrickDecoy)),
            new Ability(typeof(XmlTrickstersGambit)),
            new Ability(typeof(XmlTrueFear)),
            new Ability(typeof(XmlVolcanicEruption)),
            new Ability(typeof(XmlVortexPull)),
            new Ability(typeof(XmlWaterVortex)),
            new Ability(typeof(XmlWhirlwindFury)),
            new Ability(typeof(XmlWindBlast)),
            // Add more abilities here as needed...
        };

        public class Ability
        {
            public Type AbilityType { get; set; }
            public object[] Parameters { get; set; }

            public Ability(Type type, params object[] parameters)
            {
                AbilityType = type;
                Parameters = parameters;
            }
        }

        // Constructor for XmlAttachable
        [Attachable]
        public XmlRandomAbility() : this(1, 5) { }

        // Custom constructor to define the min/max number of abilities
        [Attachable]
        public XmlRandomAbility(int minAbilities, int maxAbilities)
        {
            m_MinAbilities = minAbilities;
            m_MaxAbilities = maxAbilities;
        }

        // OnAttach gets called when the attachment is applied to the mobile or item
        public override void OnAttach()
        {
            base.OnAttach();

            Mobile mob = AttachedTo as Mobile;

            if (mob != null)
            {
                AttachRandomAbilities(mob);
                Delete(); // Self-delete after abilities are attached
            }
        }

        private void AttachRandomAbilities(Mobile mob)
        {
            // Determine the number of abilities to attach
            int numAbilities = Utility.RandomMinMax(m_MinAbilities, m_MaxAbilities);
            HashSet<int> selectedAbilities = new HashSet<int>();

            while (selectedAbilities.Count < numAbilities)
            {
                int index = Utility.Random(AvailableAbilities.Count);
                selectedAbilities.Add(index);
            }

            // Attach each selected ability to the mobile
            foreach (var index in selectedAbilities)
            {
                Ability ability = AvailableAbilities[index];

                try
                {
                    // Check if the ability requires parameters
                    if (ability.Parameters != null && ability.Parameters.Length > 0)
                    {
                        // Instantiate the attachment with parameters
                        XmlAttachment abilityAttachment = (XmlAttachment)Activator.CreateInstance(ability.AbilityType, ability.Parameters);
                        XmlAttach.AttachTo(mob, abilityAttachment);
                    }
                    else
                    {
                        // Instantiate the attachment without parameters
                        XmlAttachment abilityAttachment = (XmlAttachment)Activator.CreateInstance(ability.AbilityType);
                        XmlAttach.AttachTo(mob, abilityAttachment);
                    }
                }
                catch (Exception ex)
                {
                    mob.SendMessage("An error occurred while attaching the ability: " + ex.Message);
                }
            }

            mob.SendMessage("You have been granted random abilities!");
        }

        // Serialization method
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
            writer.Write(m_MinAbilities);
            writer.Write(m_MaxAbilities);
        }

        // Deserialization method
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_MinAbilities = reader.ReadInt();
            m_MaxAbilities = reader.ReadInt();
        }

        // This method provides information when the attachment is identified
        public override string OnIdentify(Mobile from)
        {
            return $"Random Ability Attachment:\n" +
                   $"- Min Abilities: {m_MinAbilities}\n" +
                   $"- Max Abilities: {m_MaxAbilities}\n" +
                   $"(Automatically attaches random abilities when applied.)";
        }
    }
}
